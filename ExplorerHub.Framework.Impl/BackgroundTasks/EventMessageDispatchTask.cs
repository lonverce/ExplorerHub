using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using MindLab.Messaging;

namespace ExplorerHub.Framework.BackgroundTasks
{
    /// <summary>
    /// 负责把由<see cref="IEventBus"/>发布的事件推送给相关的<see cref="IEventSubscriber"/>
    /// </summary>
    internal sealed class EventMessageDispatchTask : IBackgroundTask
    {
        private readonly IUiDispatcher _uiDispatcher;
        private readonly IMessageRouter<IEventData> _messageRouter;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly IReadOnlyDictionary<string, SubscriberCollection> _subscribers;
        private Task _bgTask = Task.CompletedTask;
        private IAsyncDisposable _binding;
        private readonly MessageQueue<IEventData> _queue = new MessageQueue<IEventData>();

        private class SubscriberCollection
        {
            private static readonly Func<Owned<IEventSubscriber>>[] _default =
                Array.Empty<Func<Owned<IEventSubscriber>>>();

            public Func<Owned<IEventSubscriber>>[] UiSubscribers { get; set; } = _default;

            public Func<Owned<IEventSubscriber>>[] BackgroundSubscribers { get; set; } = _default;
        }

        public EventMessageDispatchTask(
            IUiDispatcher uiDispatcher,
            IMessageRouter<IEventData> messageRouter, 
            IEnumerable<Meta<Func<Owned<IEventSubscriber>>>> subscribers)
        {
            _uiDispatcher = uiDispatcher;
            _messageRouter = messageRouter;
            
            _subscribers = subscribers.Select(meta =>
                {
                    if (!meta.Metadata.TryGetValue(nameof(EventSubscriberAttribute), out var attr)
                        || !(attr is EventSubscriberAttribute val))
                    {
                        return null;
                    }

                    return new
                    {
                        factory = meta.Value,
                        describe = val
                    };
                })
                .Where(meta => meta!=null)
                .GroupBy(meta => meta.describe.EventName)
                .ToDictionary(grouping => grouping.Key, grouping =>
                {
                    var subDic = grouping.GroupBy(arg => arg.describe.UiHandle)
                        .ToDictionary(subGroup => subGroup.Key, 
                            subGroup => subGroup.Select(arg =>arg.factory) .ToArray());
                    var collection = new SubscriberCollection();

                    if (subDic.TryGetValue(true, out var uiSubscribers))
                    {
                        collection.UiSubscribers = uiSubscribers;
                    }

                    if (subDic.TryGetValue(false, out var bgSubscribers))
                    {
                        collection.BackgroundSubscribers = bgSubscribers;
                    }

                    return collection;
                });
        }

        public async Task StartAsync()
        {
            _binding = await _queue.BindAsync(string.Empty, _messageRouter);
            _bgTask = Task.Run(DispatchTask);
        }

        private async Task DispatchTask()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                try
                {
                    var msg = await _queue.TakeMessageAsync(_tokenSource.Token);
                    await HandleMessageAsync(msg.Payload);
                }
                catch (OperationCanceledException e) when (e.CancellationToken == _tokenSource.Token)
                {
                    break;
                }
            }
        }

        private async ValueTask HandleMessageAsync(IEventData data)
        {
            if (!_subscribers.TryGetValue(data.Name, out var subscriberCollection))
            {
                return;
            }

            var bgTasks = subscriberCollection.BackgroundSubscribers.Select(subFac =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        await using var sb = subFac();
                        await sb.Value.HandleAsync(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }).ToArray();
            
            await _uiDispatcher.InvokeAsync(async () =>
            {
                foreach (var uiSubscriber in subscriberCollection.UiSubscribers)
                {
                    try
                    {
                        await using var usb = uiSubscriber();
                        await usb.Value.HandleAsync(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
            
            await Task.WhenAll(bgTasks);
        }

        public async Task StopAsync()
        {
            _tokenSource.Cancel();
            await _bgTask;
            await _binding.DisposeAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using MindLab.Messaging;

namespace ExplorerHub.BackgroundTasks
{
    /// <summary>
    /// 负责把由<see cref="IEventBus"/>发布的事件推送给相关的<see cref="IEventSubscriber"/>
    /// </summary>
    public class EventMessageDispatchTask : IBackgroundTask
    {
        private readonly App _app;
        private readonly IMessageRouter<IEventData> _messageRouter;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly IReadOnlyDictionary<string, SubscriberCollection> _subscribers;
        private readonly ManualResetEvent _endEvt = new ManualResetEvent(false);

        private class SubscriberCollection
        {
            private static readonly Func<Owned<IEventSubscriber>>[] _default =
                Array.Empty<Func<Owned<IEventSubscriber>>>();

            public Func<Owned<IEventSubscriber>>[] UiSubscribers { get; set; } = _default;

            public Func<Owned<IEventSubscriber>>[] BackgroundSubscribers { get; set; } = _default;
        }

        public EventMessageDispatchTask(
            App app,
            IMessageRouter<IEventData> messageRouter, 
            IEnumerable<Meta<Func<Owned<IEventSubscriber>>>> subscribers)
        {
            _app = app;
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

        public void Start()
        {
            using var initEvt = new ManualResetEvent(false);

            var tsk = Task.Run(async () =>
            {
                MessageQueue<IEventData> queue;
                IAsyncDisposable binding;

                try
                {
                    queue = new MessageQueue<IEventData>();
                    binding = await queue.BindAsync(string.Empty, _messageRouter);
                }
                finally
                {
                    initEvt.Set();
                }

                await using var _ = binding;
                try
                {
                    while (!_tokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            var msg = await queue.TakeMessageAsync(_tokenSource.Token);
                            await HandleMessageAsync(msg.Payload);
                        }
                        catch (OperationCanceledException e) when (e.CancellationToken == _tokenSource.Token)
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    _endEvt.Set();
                }
            });

            initEvt.WaitOne();

            if (tsk.IsFaulted)
            {
                throw tsk.Exception;
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
                return Task.Run(() =>
                {
                    try
                    {
                        using var sb = subFac();
                        sb.Value.Handle(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }).ToArray();

            await _app.Dispatcher.InvokeAsync(() =>
            {
                foreach (var uiSubscriber in subscriberCollection.UiSubscribers)
                {
                    using var usb = uiSubscriber();
                    usb.Value.Handle(data);
                }
            });
            
            await Task.WhenAll(bgTasks);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _endEvt.WaitOne(TimeSpan.FromSeconds(3));
        }
    }
}

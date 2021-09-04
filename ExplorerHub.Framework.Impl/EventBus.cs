using System.Diagnostics;
using System.Threading.Tasks;
using MindLab.Threading;

namespace ExplorerHub.Framework
{
    internal sealed class EventBus : IEventBus
    {
        private readonly AsyncBlockingCollection<IEventData> _queue;

        public EventBus(AsyncBlockingCollection<IEventData> queue)
        {
            _queue = queue;
        }

        public void PublishEvent(IEventData eventData)
        {
            if (_queue.TryAdd(eventData))
            {
                return;
            }

            var process = Process.GetCurrentProcess();
            process.Kill();
        }

        public async Task PublishEventAsync(IEventData eventData)
        {
            PublishEvent(eventData);
            await Task.CompletedTask;
        }
    }
}

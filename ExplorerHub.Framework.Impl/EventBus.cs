using System.Threading.Tasks;
using MindLab.Messaging;

namespace ExplorerHub.Framework
{
    internal sealed class EventBus : IEventBus
    {
        private readonly IMessagePublisher<IEventData> _publisher;

        public EventBus(IMessagePublisher<IEventData> publisher)
        {
            _publisher = publisher;
        }

        public void PublishEvent(IEventData eventData)
        {
            PublishEventAsync(eventData);
        }

        public async Task PublishEventAsync(IEventData eventData)
        {
            if (eventData == null)
            {
                return;
            }

            await _publisher.PublishMessageAsync(string.Empty, eventData);
        }
    }
}

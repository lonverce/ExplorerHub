using MindLab.Messaging;

namespace ExplorerHub.Infrastructure
{
    public class EventBus : IEventBus
    {
        private readonly IMessagePublisher<IEventData> _publisher;

        public EventBus(IMessagePublisher<IEventData> publisher)
        {
            _publisher = publisher;
        }

        public void PublishEvent(IEventData eventData)
        {
            if (eventData == null)
            {
                return;
            }

            _publisher.PublishMessageAsync(string.Empty, eventData);
        }
    }
}

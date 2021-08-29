using ExplorerHub.Events;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(SystemColorChangedEventData.EventName, UiHandle = true)]
    public class SystemColorEventSubscriber : IEventSubscriber
    {
        private readonly App _app;

        public SystemColorEventSubscriber(App app)
        {
            _app = app;
        }

        public void Handle(IEventData eventData)
        {
            var data = (SystemColorChangedEventData) eventData;
            var color = data.NewColor;
            _app.SetAppBackground(color);
        }
    }
}

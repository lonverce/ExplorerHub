using ExplorerHub.Events;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(UserNotificationEventData.EventName, UiHandle = true)]
    public class UserNotificationEventSubscriber : IEventSubscriber
    {
        private readonly HiddenMainWindow _window;

        public UserNotificationEventSubscriber(App app)
        {
            _window = app.MainWindow as HiddenMainWindow;
        }

        public void Handle(IEventData eventData)
        {
            var data = (UserNotificationEventData) eventData;
            _window.ShowUserMessage(data.Message, data.Title, data.Icon);
        }
    }
}

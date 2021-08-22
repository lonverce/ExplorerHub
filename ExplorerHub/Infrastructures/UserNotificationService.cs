using System;
using System.Windows.Forms;
using ExplorerHub.Events;

namespace ExplorerHub.Infrastructures
{
    public class UserNotificationService:IUserNotificationService
    {
        private readonly IEventBus _eventBus;

        public UserNotificationService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Notify(string message, string title, ToolTipIcon icon)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            _eventBus.PublishEvent(new UserNotificationEventData(message)
            {
                Title = title,
                Icon = icon
            });
        }
    }
}

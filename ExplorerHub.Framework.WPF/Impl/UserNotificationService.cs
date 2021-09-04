using System;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal sealed class UserNotificationService : IUserNotificationService
    {
        private readonly IEventBus _eventBus;

        public UserNotificationService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Notify(string message, string title, NotificationLevel level = NotificationLevel.Min, bool isAsync = true)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            _eventBus.PublishEvent(new UserNotificationEventData(message)
            {
                Title = title,
                Level = level,
                IsAsync = isAsync
            });
        }
    }
}

using System;
using System.Threading.Tasks;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal sealed class UserNotificationService : IUserNotificationService
    {
        private readonly IEventBus _eventBus;

        public UserNotificationService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task NotifyAsync(string message, string title, NotificationLevel level, bool isAsync)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            await _eventBus.PublishEventAsync(new UserNotificationEventData(message)
            {
                Title = title,
                Level = level,
                IsAsync = isAsync
            });
        }
    }
}

using System;

namespace ExplorerHub.Framework.WPF.Impl
{
    public class DefaultCommandExceptionHandler : ICommandExceptionHandler
    {
        private readonly IUserNotificationService _notificationService;

        public DefaultCommandExceptionHandler(IUserNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void HandleException(Exception e)
        {
            _notificationService.Notify(e.Message, "ExplorerHub", NotificationLevel.Error);
        }
    }
}

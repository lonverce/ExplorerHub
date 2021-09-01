using System;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.WPF
{
    internal class CommandInterceptor : IInterceptor
    {
        private readonly IUserNotificationService _notificationService;

        public CommandInterceptor(IUserNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _notificationService.Notify(e.Message, e.GetType().FullName, NotificationLevel.Error, false);
            }
        }
    }
}

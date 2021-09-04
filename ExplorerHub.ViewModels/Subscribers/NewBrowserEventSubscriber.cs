using System.Threading.Tasks;
using ExplorerHub.Events;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Subscribers
{
    [EventSubscriber(NewExplorerEventData.EventName, UiHandle = true)]
    public class NewBrowserEventSubscriber : IEventSubscriber
    {
        private readonly IAbsorbService _absorbService;
        private readonly IUserNotificationService _notificationService;

        public NewBrowserEventSubscriber(
            IAbsorbService absorbService,
            IUserNotificationService notificationService)
        {
            _absorbService = absorbService;
            _notificationService = notificationService;
        }

        public async Task HandleAsync(IEventData eventData)
        {
            var data = (NewExplorerEventData) eventData;
            var shellBrowser = data.Window;

            try
            {
                await _absorbService.AbsorbAsync(shellBrowser);
            }
            catch (AbsorbFailureException e)
            {
                shellBrowser.Close();
                _notificationService.Notify(e.Message, "ExplorerHub", NotificationLevel.Warn);
            }
        }
    }
}

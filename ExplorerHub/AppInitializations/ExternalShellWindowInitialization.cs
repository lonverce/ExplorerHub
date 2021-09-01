using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels;

namespace ExplorerHub.AppInitializations
{
    public class ExternalShellWindowInitialization : IAppInitialization
    {
        private readonly IAbsorbService _absorbService;
        private readonly IUserNotificationService _notificationService;
        private readonly IShellWindowsManager _shellWindowsManager;

        public ExternalShellWindowInitialization(
            IAbsorbService absorbService,
            IUserNotificationService notificationService,
            IShellWindowsManager shellWindowsManager)
        {
            _absorbService = absorbService;
            _notificationService = notificationService;
            _shellWindowsManager = shellWindowsManager;
        }

        public void InitializeAppComponents()
        {
            foreach (var shellWindow in _shellWindowsManager.GetCurrentWindows())
            {
                try
                {
                    _absorbService.Absorb(shellWindow);
                }
                catch (AbsorbFailureException e)
                {
                    shellWindow.Close();
                    _notificationService.Notify(e.Message, "ExplorerHub", NotificationLevel.Warn);
                }
            }
        }
    }
}

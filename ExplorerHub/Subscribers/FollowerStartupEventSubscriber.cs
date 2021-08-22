using System.Windows.Forms;
using ExplorerHub.Events;
using ExplorerHub.ViewModels;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(FollowerStartupEventData.EventName, UiHandle = true)]
    public class FollowerStartupEventSubscriber : IEventSubscriber
    {
        private readonly IUserNotificationService _notificationService;
        private readonly IKnownFolderManager _folderManager;
        private readonly IHubWindowsManager _windowsManager;

        public FollowerStartupEventSubscriber(
            IUserNotificationService notificationService,
            IKnownFolderManager folderManager, 
            IHubWindowsManager windowsManager)
        {
            _notificationService = notificationService;
            _folderManager = folderManager;
            _windowsManager = windowsManager;
        }

        public void Handle(IEventData eventData)
        {
            var data = (FollowerStartupEventData) eventData;
            var args = data.Args;

            if (args.Length == 0)
            {
                var hub = _windowsManager.GetOrCreateActiveHubWindow();
                if (hub.Explorers.Count == 0)
                {
                    hub.AddBrowserCommand.Execute();
                }
            }
            else
            {
                var firstPath = args[0];
                if (!_folderManager.TryParse(firstPath, out var shellObject))
                {
                    _notificationService.Notify($"错误路径：'{firstPath}'", "ExplorerHub", ToolTipIcon.Error);
                    return;
                }

                _windowsManager.GetOrCreateActiveHubWindow().AddBrowserCommand.Execute(shellObject);
            }
        }
    }
}

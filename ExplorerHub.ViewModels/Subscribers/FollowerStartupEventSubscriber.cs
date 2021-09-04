using System.Threading.Tasks;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Subscribers
{
    [EventSubscriber(FollowerStartupEventData.EventName, UiHandle = true)]
    public class FollowerStartupEventSubscriber : IEventSubscriber
    {
        private readonly IUserNotificationService _notificationService;
        private readonly IShellUrlParser _parser;
        private readonly IHubWindowsManager _windowsManager;

        public FollowerStartupEventSubscriber(
            IUserNotificationService notificationService,
            IShellUrlParser parser, 
            IHubWindowsManager windowsManager)
        {
            _notificationService = notificationService;
            _parser = parser;
            _windowsManager = windowsManager;
        }

        public async Task HandleAsync(IEventData eventData)
        {
            var data = (FollowerStartupEventData) eventData;
            var args = data.Args;

            if (args.Length == 0)
            {
                var hub = _windowsManager.GetOrCreateActiveHubWindow();
                if (hub.Explorers.Count == 0)
                {
                    hub.AddBrowser.Execute();
                }
            }
            else
            {
                var firstPath = args[0];
                if (!_parser.TryParse(firstPath, out var shellObject))
                {
                    _notificationService.Notify($"错误路径：'{firstPath}'", 
                        "ExplorerHub", NotificationLevel.Error);
                    return;
                }

                _windowsManager.GetOrCreateActiveHubWindow().AddBrowser.Execute(shellObject);
            }

            await Task.CompletedTask;
        }
    }
}

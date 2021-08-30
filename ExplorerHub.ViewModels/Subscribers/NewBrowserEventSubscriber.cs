using System;
using ExplorerHub.Events;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Subscribers
{
    [EventSubscriber(NewExplorerEventData.EventName, UiHandle = true)]
    public class NewBrowserEventSubscriber : IEventSubscriber
    {
        private readonly IShellUrlParser _parser;
        private readonly IHubWindowsManager _windowsManager;
        private readonly IUserNotificationService _notificationService;

        public NewBrowserEventSubscriber(
            IShellUrlParser parser,
            IHubWindowsManager windowsManager, 
            IUserNotificationService notificationService)
        {
            _parser = parser;
            _windowsManager = windowsManager;
            _notificationService = notificationService;
        }

        public void Handle(IEventData eventData)
        {
            var data = (NewExplorerEventData) eventData;
            var shellBrowser = data.Window;

            ShellObject target;

            if (!string.IsNullOrWhiteSpace(shellBrowser.LocationUrl))
            {
                var uri = new Uri(shellBrowser.LocationUrl);
                if (!uri.IsFile)
                {
                    _notificationService.Notify(
                        $"无法识别URL: {shellBrowser.LocationUrl}",
                        "ExplorerHub", NotificationLevel.Warn);
                    return;
                }
                
                try
                {
                    target = ShellObject.FromParsingName(shellBrowser.LocationUrl);
                }
                catch (Exception e)
                {
                    _notificationService.Notify($"无法导航到: {shellBrowser.LocationUrl}\n原因: {e.Message}",
                        "错误", NotificationLevel.Error);
                    return;
                }
            }
            else if (_parser.KnownFolders.TryGetValue(shellBrowser.LocationName, out var objs))
            {
                target = objs[0];
            }
            else
            {
                _notificationService.Notify(
                    $"无法识别路径: {shellBrowser.LocationName}",
                    "ExplorerHub", NotificationLevel.Warn);
                target = _parser.Default;
            }

            shellBrowser.Close();
            _windowsManager.GetOrCreateActiveHubWindow().AddBrowser.Execute(target);
        }
    }
}

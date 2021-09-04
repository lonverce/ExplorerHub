using System;
using System.Threading.Tasks;
using ExplorerHub.Framework.WPF;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    public class AbsorbService : IAbsorbService
    {
        private readonly IHubWindowsManager _hubWindows;
        private readonly IUserNotificationService _notificationService;
        private readonly IShellUrlParser _parser;

        public AbsorbService(IHubWindowsManager hubWindows, IUserNotificationService notificationService, IShellUrlParser parser)
        {
            _hubWindows = hubWindows;
            _notificationService = notificationService;
            _parser = parser;
        }

        public async Task<ExplorerViewModel> AbsorbAsync(IShellWindow shellBrowser)
        {
            ShellObject target;

            if (!string.IsNullOrWhiteSpace(shellBrowser.LocationUrl))
            {
                var uri = new Uri(shellBrowser.LocationUrl);
                if (!uri.IsFile)
                {
                    throw new AbsorbFailureException(shellBrowser.LocationUrl, "该导航路径不是合法的文件系统路径");
                }

                try
                {
                    target = ShellObject.FromParsingName(shellBrowser.LocationUrl);
                }
                catch (Exception e)
                {
                    throw new AbsorbFailureException(shellBrowser.LocationUrl, e.Message);
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
            await Task.CompletedTask;
            return _hubWindows.GetOrCreateActiveHubWindow().AddBrowser.Execute(target);
        }
    }
}

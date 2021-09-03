using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 处理进程启动参数，按照参数创建Explorer
    /// </summary>
    public class StartupArgInitialization : IAppInitialization
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;
        private readonly IUserNotificationService _notificationService;
        private readonly IShellUrlParser _parser;
        private readonly SplashScreen _splash;

        public StartupArgInitialization(
            App app, 
            IHubWindowsManager windowsManager,
            IUserNotificationService notificationService,
            IShellUrlParser parser,
            SplashScreen splash)
        {
            _app = app;
            _windowsManager = windowsManager;
            _notificationService = notificationService;
            _parser = parser;
            _splash = splash;
        }

        public async Task InitializeAppComponentsAsync()
        {
            var e = _app.StartupEventArgs;

            if (e.Args.Any())
            {
                if (_parser.TryParse(e.Args[0], out var initShellObj))
                {
                    _windowsManager.GetOrCreateActiveHubWindow().AddBrowser.Execute(initShellObj);
                }
                else
                {
                    await _notificationService.NotifyAsync($"启动参数有误, 无法导航到 '{e.Args[0]}'", "ExplorerHub", NotificationLevel.Warn);
                }
            }
            else
            {
                var wnd = _windowsManager.GetOrCreateActiveHubWindow();
                if (!wnd.Explorers.Any())
                {
                    wnd.AddBrowser.Execute(_parser.Default);
                }
            }

            _splash.Close(TimeSpan.Zero);
        }

        public Task ReleaseAppComponentAsync()
        {
            return Task.CompletedTask;
        }
    }
}

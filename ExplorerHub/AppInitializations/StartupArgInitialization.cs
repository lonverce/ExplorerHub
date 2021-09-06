using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 处理进程启动参数，按照参数创建Explorer
    /// </summary>
    public class StartupArgInitialization : IAppInitialization
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;
        private readonly IManagedObjectRepository<ExplorerHubViewModel> _hubRepository;
        private readonly IUserNotificationService _notificationService;
        private readonly IShellUrlParser _parser;
        private readonly SplashScreen _splash;

        public StartupArgInitialization(
            App app, 
            IHubWindowsManager windowsManager,
            IManagedObjectRepository<ExplorerHubViewModel> hubRepository,
            IUserNotificationService notificationService,
            IShellUrlParser parser,
            SplashScreen splash)
        {
            _app = app;
            _windowsManager = windowsManager;
            _hubRepository = hubRepository;
            _notificationService = notificationService;
            _parser = parser;
            _splash = splash;
        }

        public async Task InitializeAppComponentsAsync()
        {
            await Task.CompletedTask;

            var e = _app.Options;

            if (!string.IsNullOrWhiteSpace(e.Directory))
            {
                if (_parser.TryParse(e.Directory, out var initShellObj))
                {
                    _windowsManager.GetOrCreateActiveHubWindow().AddBrowser.Execute(initShellObj);
                    _splash.Close(TimeSpan.Zero);
                    return;
                }

                _notificationService.Notify($"启动参数有误, 无法导航到 '{e.Directory}'", "ExplorerHub", NotificationLevel.Warn);
            }

            if(!e.MiniStart)
            {
                var wnd = _windowsManager.GetOrCreateActiveHubWindow();
                if (!wnd.Explorers.Any())
                {
                    wnd.AddBrowser.Execute(_parser.Default);
                }
            }

            if (!_hubRepository.Any())
            {
                _notificationService.Notify("已最小化到托盘", "ExplorerHub", NotificationLevel.Info);
            }

            _splash.Close(TimeSpan.Zero);
        }

        public Task ReleaseAppComponentAsync()
        {
            return Task.CompletedTask;
        }
    }
}

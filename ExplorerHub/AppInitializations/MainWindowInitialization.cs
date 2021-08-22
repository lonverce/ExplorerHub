using System.Windows;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 创建应用程序窗体
    /// </summary>
    public class MainWindowInitialization : IAppInitialization
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;

        public MainWindowInitialization(App app, IHubWindowsManager windowsManager)
        {
            _app = app;
            _windowsManager = windowsManager;
        }

        public void InitializeAppComponents()
        {
            var hiddenWnd = new HiddenMainWindow(_app, _windowsManager);
            _app.MainWindow = hiddenWnd;

            hiddenWnd.Visibility = Visibility.Hidden;
            hiddenWnd.WindowState = WindowState.Minimized;
            hiddenWnd.Show();
        }
    }
}

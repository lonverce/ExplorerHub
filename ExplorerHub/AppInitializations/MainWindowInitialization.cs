﻿using System.Threading.Tasks;
using System.Windows;
using ExplorerHub.Framework;
using ExplorerHub.ViewModels;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 创建应用程序窗体
    /// </summary>
    public class MainWindowInitialization : IAppInitialization
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;
        private readonly ISystemColorManager _colorManager;

        public MainWindowInitialization(App app, IHubWindowsManager windowsManager, ISystemColorManager colorManager)
        {
            _app = app;
            _windowsManager = windowsManager;
            _colorManager = colorManager;
        }
        
        public Task InitializeAppComponentsAsync()
        {
            _app.SetAppBackground(_colorManager.GetSystemColor());
            var hiddenWnd = new HiddenMainWindow(_app, _windowsManager);
            _app.MainWindow = hiddenWnd;

            hiddenWnd.Visibility = Visibility.Hidden;
            hiddenWnd.WindowState = WindowState.Minimized;
            hiddenWnd.Show();
            return Task.CompletedTask;
        }

        public Task ReleaseAppComponentAsync()
        {
            _app.MainWindow?.Close();
            return Task.CompletedTask;
        }
    }
}

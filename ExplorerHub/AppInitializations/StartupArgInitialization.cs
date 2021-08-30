using System;
using System.Linq;
using System.Windows;
using ExplorerHub.ViewModels;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.AppInitializations
{
    /// <summary>
    /// 处理进程启动参数，按照参数创建Explorer
    /// </summary>
    public class StartupArgInitialization : IAppInitialization
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;
        private readonly IShellUrlParser _parser;
        private readonly SplashScreen _splash;

        public StartupArgInitialization(
            App app, 
            IHubWindowsManager windowsManager,
            IShellUrlParser parser,
            SplashScreen splash)
        {
            _app = app;
            _windowsManager = windowsManager;
            _parser = parser;
            _splash = splash;
        }

        public void InitializeAppComponents()
        {
            ShellObject initShellObj = null;

            var e = _app.StartupEventArgs;

            if (e.Args.Any())
            {
                if (!_parser.TryParse(e.Args[0], out initShellObj))
                {
                    initShellObj = _parser.Default;
                }
            }

            _windowsManager.CreateHubWindow().AddBrowser.Execute(initShellObj);
            _splash.Close(TimeSpan.Zero);
        }
    }
}

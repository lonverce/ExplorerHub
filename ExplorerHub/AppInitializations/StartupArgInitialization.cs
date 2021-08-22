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
        private readonly IKnownFolderManager _folderManager;
        private readonly SplashScreen _splash;

        public StartupArgInitialization(
            App app, 
            IHubWindowsManager windowsManager,
            IKnownFolderManager folderManager,
            SplashScreen splash)
        {
            _app = app;
            _windowsManager = windowsManager;
            _folderManager = folderManager;
            _splash = splash;
        }

        public void InitializeAppComponents()
        {
            ShellObject initShellObj = null;

            var e = _app.StartupEventArgs;

            if (e.Args.Any())
            {
                if (!_folderManager.TryParse(e.Args[0], out initShellObj))
                {
                    initShellObj = _folderManager.Default;
                }
            }

            _windowsManager.CreateHubWindow().AddBrowserCommand.Execute(initShellObj);
            _splash.Close(TimeSpan.Zero);
        }
    }
}

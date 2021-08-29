using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using SHDocVw;

namespace ExplorerHub.Infrastructures
{
    public class ShellWindowManager : IShellWindowsManager, IDisposable
    {
        private readonly IShellUrlParser _parser;
        private readonly IUserNotificationService _notificationService;
        public event EventHandler WindowCreated;
        private readonly ShellWindowsClass _shell = new ShellWindowsClass();

        public ShellWindowManager(
            IShellUrlParser parser, 
            IUserNotificationService notificationService)
        {
            _parser = parser;
            _notificationService = notificationService;
            _shell.WindowRegistered += ShellOnWindowRegistered;
        }

        private void ShellOnWindowRegistered(int cookie)
        {
            WindowCreated?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<IShellWindow> GetCurrentWindows()
        {
            foreach (InternetExplorer shellBrowser in _shell)
            {
                if (!string.Equals("explorer.exe", Path.GetFileName(shellBrowser.FullName), StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                //var parsingName = shellBrowser.LocationName;
                if (!string.IsNullOrWhiteSpace(shellBrowser.LocationURL))
                {
                    var uri = new Uri(shellBrowser.LocationURL);
                    if (!uri.IsFile)
                    {
                        _notificationService.Notify(
                            $"无法识别URL: {shellBrowser.LocationURL}",
                            "ExplorerHub", NotificationLevel.Warn);
                        continue;
                    }

                    ShellObject target;

                    try
                    {
                        target = ShellObject.FromParsingName(shellBrowser.LocationURL);
                    }
                    catch (Exception e)
                    {
                        _notificationService.Notify($"无法导航到: {shellBrowser.LocationURL}\n原因: {e.Message}",
                            "错误", NotificationLevel.Error);
                        continue;
                    }

                    yield return new ShellWindowController(target, shellBrowser);
                }
                else if (_parser.KnownFolders.TryGetValue(shellBrowser.LocationName, out var objs))
                {
                    yield return new ShellWindowController(objs[0], shellBrowser);
                }
                else
                {
                    _notificationService.Notify(
                        $"无法识别路径: {shellBrowser.LocationName}", 
                        "ExplorerHub", NotificationLevel.Warn);
                    yield return new ShellWindowController(_parser.Default, shellBrowser);
                }
            }
        }

        public void Dispose()
        {
            _shell.WindowRegistered -= ShellOnWindowRegistered;
        }

        public class ShellWindowController : IShellWindow
        {
            private readonly IWebBrowser2 _nativeBrowser;
            public ShellObject Target { get; }

            public ShellWindowController(ShellObject target, IWebBrowser2 nativeBrowser)
            {
                _nativeBrowser = nativeBrowser;
                Target = target;
            }

            public void Close()
            {
                _nativeBrowser.Stop();
                _nativeBrowser.Quit();
            }
        }
    }
}

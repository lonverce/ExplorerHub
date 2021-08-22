using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using SHDocVw;

namespace ExplorerHub.Infrastructures
{
    public class ShellWindowManager : IShellWindowsManager, IDisposable
    {
        private readonly IKnownFolderManager _folderManager;
        private readonly IUserNotificationService _notificationService;
        public event EventHandler WindowCreated;
        private readonly ShellWindowsClass _shell = new ShellWindowsClass();

        public ShellWindowManager(
            IKnownFolderManager folderManager, 
            IUserNotificationService notificationService)
        {
            _folderManager = folderManager;
            _notificationService = notificationService;
            _shell.WindowRegistered += ShellOnWindowRegistered;
        }

        private void ShellOnWindowRegistered(int cookie)
        {
            WindowCreated?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<IShellWindow> GetCurrentWindows()
        {
            foreach (IWebBrowser2 shellBrowser in _shell)
            {
                if (!string.Equals("explorer.exe", Path.GetFileName(shellBrowser.FullName), StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                var parsingName = shellBrowser.LocationName;
                if (!string.IsNullOrWhiteSpace(shellBrowser.LocationURL))
                {
                    var uri = new Uri(shellBrowser.LocationURL);
                    if (!uri.IsFile)
                    {
                        _notificationService.Notify(
                            $"无法识别URL: {shellBrowser.LocationURL}",
                            "ExplorerHub", ToolTipIcon.Warning);
                        continue;
                    }

                    parsingName = string.Join($"{Path.DirectorySeparatorChar}", uri.Segments.Select(seg => Uri.UnescapeDataString(seg.TrimEnd('/'))).Where(seg => !string.IsNullOrEmpty(seg)));
                    yield return new ShellWindowController(ShellObject.FromParsingName(parsingName), shellBrowser);
                }
                else if (_folderManager.Folders.TryGetValue(shellBrowser.LocationName, out var objs))
                {
                    yield return new ShellWindowController(objs[0], shellBrowser);
                }
                else
                {
                    _notificationService.Notify(
                        $"无法识别路径: {shellBrowser.LocationName}", 
                        "ExplorerHub", ToolTipIcon.Warning);
                    yield return new ShellWindowController(_folderManager.Default, shellBrowser);
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
                _nativeBrowser.Quit();
            }
        }
    }
}

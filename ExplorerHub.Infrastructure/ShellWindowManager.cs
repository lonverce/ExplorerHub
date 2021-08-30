using System;
using System.Collections.Generic;
using System.IO;
using SHDocVw;

namespace ExplorerHub.Infrastructure
{
    public class ShellWindowManager : IShellWindowsManager, IDisposable
    {
        public event EventHandler WindowCreated;
        private readonly ShellWindowsClass _shell = new ShellWindowsClass();

        public ShellWindowManager()
        {
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

                yield return new ShellWindowController(shellBrowser);
            }
        }

        public void Dispose()
        {
            _shell.WindowRegistered -= ShellOnWindowRegistered;
        }

        public class ShellWindowController : IShellWindow
        {
            private readonly IWebBrowser2 _nativeBrowser;
            
            public string LocationName => _nativeBrowser.LocationName;

            public string LocationUrl => _nativeBrowser.LocationURL;

            public ShellWindowController(IWebBrowser2 nativeBrowser)
            {
                _nativeBrowser = nativeBrowser;
            }

            public void Close()
            {
                _nativeBrowser.Stop();
                _nativeBrowser.Quit();
            }
        }
    }
}

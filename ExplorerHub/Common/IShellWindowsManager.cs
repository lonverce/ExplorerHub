using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub
{
    public interface IShellWindowsManager
    {
        event EventHandler WindowCreated;

        IEnumerable<IShellWindow> GetCurrentWindows();
    }

    public interface IShellWindow
    {
        ShellObject Target { get; }

        void Close();
    }
}
using System;
using System.Collections.Generic;

namespace ExplorerHub.ViewModels
{
    public interface IShellWindowsManager
    {
        event EventHandler WindowCreated;

        IEnumerable<IShellWindow> GetCurrentWindows();
    }

    public interface IShellWindow
    {
        string LocationName { get; }

        string LocationUrl { get; }

        IntPtr Handle { get; }

        void Close();
    }
}
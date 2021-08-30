using System;
using System.Collections.Generic;

namespace ExplorerHub
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

        void Close();
    }
}
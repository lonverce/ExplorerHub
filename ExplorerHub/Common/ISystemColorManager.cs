using System;
using System.Windows.Media;

namespace ExplorerHub
{
    public interface ISystemColorManager
    {
        Color GetSystemColor();

        event EventHandler<Color> SystemColorChanged;
    }
}
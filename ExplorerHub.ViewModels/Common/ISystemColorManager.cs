using System;
using System.Windows.Media;

namespace ExplorerHub
{
    public interface ISystemColorManager
    {
        /// <summary>
        /// 获取Win10系统主题色
        /// </summary>
        /// <returns></returns>
        Color GetSystemColor();

        /// <summary>
        /// 监听系统主题色变更
        /// </summary>
        event EventHandler<Color> SystemColorChanged;
    }
}
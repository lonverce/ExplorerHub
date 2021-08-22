using System.Windows;

namespace ExplorerHub.ViewModels
{
    public static class WindowExtensions
    {
        /// <summary>
        /// 激活窗体
        /// </summary>
        /// <param name="wnd"></param>
        public static void ActivateEx(this Window wnd)
        {
            if (wnd.Visibility != Visibility.Visible)
            {
                wnd.Visibility = Visibility.Visible;
            }

            if (wnd.WindowState == WindowState.Minimized)
            {
                wnd.WindowState = WindowState.Normal;
            }

            if (!wnd.IsActive)
            {
                wnd.Activate();
            }
        }
    }
}

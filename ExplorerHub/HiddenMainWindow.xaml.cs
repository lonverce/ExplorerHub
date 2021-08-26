using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace ExplorerHub
{
    /// <summary>
    /// HiddenMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HiddenMainWindow
    {
        private readonly App _app;
        private readonly IHubWindowsManager _windowsManager;
        private readonly NotifyIcon _notifyIcon;

        public HiddenMainWindow(App app, IHubWindowsManager windowsManager)
        {
            _app = app;
            _windowsManager = windowsManager;
            InitializeComponent();

            _notifyIcon = new NotifyIcon
            {
                Text = "ExplorerHub",
                BalloonTipText = "已最小化到托盘",
                Visible = true,
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath)
            };

            _notifyIcon.MouseDoubleClick += OnDisplayBtnClick;
            var menuItem = new MenuItem("退出");
            menuItem.Click += OnExitBtnClick;
            _notifyIcon.ContextMenu = new ContextMenu(new[] { menuItem });
        }

        public void ShowUserMessage(string message, string title = "提示", ToolTipIcon icon = ToolTipIcon.Info)
        {
            _notifyIcon.ShowBalloonTip(1000, title, message, icon);
        }

        private void OnExitBtnClick(object sender, EventArgs e)
        {
            foreach (var window in _app.HubWindows.ToArray())
            {
                window.Close();
            }

            _notifyIcon.Visible = false;
            Close();
        }

        private void OnDisplayBtnClick(object sender, MouseEventArgs e)
        {
            foreach (var hubWindow in _app.Windows.OfType<ExplorerHubWindow>())
            {
                if (hubWindow.WindowState != WindowState.Minimized)
                {
                    hubWindow.Activate();
                    return;
                }

                hubWindow.WindowState = WindowState.Normal;
                hubWindow.Activate();
                return;
            }

            _windowsManager.CreateHubWindow().AddBrowser.Execute();
        }
    }
}

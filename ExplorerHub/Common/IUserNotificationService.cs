using System.Windows.Forms;

namespace ExplorerHub
{
    /// <summary>
    /// 发送用户通知
    /// </summary>
    public interface IUserNotificationService
    {
        void Notify(string message, string title, ToolTipIcon icon = ToolTipIcon.Info);
    }
}

using System.Threading.Tasks;

namespace ExplorerHub.Framework.WPF
{
    public enum NotificationLevel
    {
        Min,
        Info,
        Warn,
        Error
    }

    /// <summary>
    /// 发送用户通知
    /// </summary>
    public interface IUserNotificationService
    {
        /// <summary>
        /// 向用户发送消息通知
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="level"></param>
        /// <param name="isAsync"></param>
        Task NotifyAsync(string message, string title, NotificationLevel level = NotificationLevel.Min, bool isAsync = true);
    }
}

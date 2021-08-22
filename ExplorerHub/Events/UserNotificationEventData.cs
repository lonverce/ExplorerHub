using System.Windows.Forms;

namespace ExplorerHub.Events
{
    public class UserNotificationEventData : IEventData
    {
        /// <summary>
        /// 用户消息
        /// </summary>
        public string Message { get; }

        public const string EventName = "UserNofitication";

        public string Name => EventName;

        public UserNotificationEventData(string message)
        {
            Message = message;
        }

        public string Title { get; set; } = "通知";

        public ToolTipIcon Icon { get; set; } = ToolTipIcon.Info;
    }
}

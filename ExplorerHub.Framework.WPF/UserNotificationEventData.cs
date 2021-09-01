namespace ExplorerHub.Framework.WPF
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

        public string Title { get; set; }

        public NotificationLevel Level { get; set; }

        public bool IsAsync { get; set; } = true;
    }
}

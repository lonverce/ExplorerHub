namespace ExplorerHub.Events
{
    public class FollowerStartupEventData : IEventData
    {
        public FollowerStartupEventData(string[] args)
        {
            Args = args;
        }

        /// <summary>
        /// 启动参数
        /// </summary>
        public string[] Args { get; }

        string IEventData.Name => EventName;

        public const string EventName = "FollowerStartup";
    }
}

namespace ExplorerHub.Events
{
    /// <summary>
    /// 新建浏览器页面
    /// </summary>
    public class NewExplorerEventData : IEventData
    {
        public NewExplorerEventData(IShellWindow window)
        {
            Window = window;
        }

        public IShellWindow Window { get; }

        string IEventData.Name => EventName;

        public const string EventName = "NewExplorer";
    }
}

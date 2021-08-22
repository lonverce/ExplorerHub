using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.Events
{
    /// <summary>
    /// 新建浏览器页面
    /// </summary>
    public class NewExplorerEventData : IEventData
    {
        public NewExplorerEventData(ShellObject target)
        {
            Target = target;
        }

        public ShellObject Target { get; }

        string IEventData.Name => EventName;

        public const string EventName = "NewExplorer";
    }
}

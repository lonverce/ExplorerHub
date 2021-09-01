namespace ExplorerHub.Framework
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public interface IBackgroundTask
    {
        void Start();

        void Stop();
    }
}

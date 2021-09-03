using System.Threading.Tasks;

namespace ExplorerHub.Framework
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public interface IBackgroundTask
    {
        Task StartAsync();

        Task StopAsync();
    }
}

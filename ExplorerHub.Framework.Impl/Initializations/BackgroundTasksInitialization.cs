using System.Threading.Tasks;

namespace ExplorerHub.Framework.Initializations
{
    /// <summary>
    /// 启动所有后台任务
    /// </summary>
    internal sealed class BackgroundTasksInitialization : IAppInitialization
    {
        private readonly BackgroundTaskManager _taskManager;

        public BackgroundTasksInitialization(BackgroundTaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public async Task InitializeAppComponentsAsync()
        {
            await _taskManager.StartAsync();
        }

        public async Task ReleaseAppComponentAsync()
        {
            await _taskManager.StopAsync();
        }
    }
}

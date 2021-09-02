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

        public void InitializeAppComponents()
        {
            _taskManager.Start();
        }

        public void ReleaseAppComponent()
        {
            _taskManager.Stop();
        }
    }
}

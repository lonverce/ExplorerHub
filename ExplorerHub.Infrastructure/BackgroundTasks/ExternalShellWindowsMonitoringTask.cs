using System;
using System.Threading.Tasks;
using ExplorerHub.Events;
using ExplorerHub.Framework;

namespace ExplorerHub.Infrastructure.BackgroundTasks
{
    /// <summary>
    /// 负责拦截系统中的文件资源管理器启动，并把相关的拦截事件（<see cref="NewExplorerEventData"/>）发布到<see cref="IEventBus"/>
    /// </summary>
    public class ExternalShellWindowsMonitoringTask : IBackgroundTask
    {
        private readonly IEventBus _eventBus;
        private readonly IShellWindowsManager _windowsManager;

        public ExternalShellWindowsMonitoringTask(IEventBus eventBus, IShellWindowsManager windowsManager)
        {
            _eventBus = eventBus;
            _windowsManager = windowsManager;
        }
        
        private void WindowsManagerOnWindowCreated(object sender, EventArgs e)
        {
            foreach (var window in _windowsManager.GetCurrentWindows())
            {
                _eventBus.PublishEvent(new NewExplorerEventData(window));
            }
        }
        
        public Task StartAsync()
        {
            _windowsManager.WindowCreated += WindowsManagerOnWindowCreated;
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _windowsManager.WindowCreated -= WindowsManagerOnWindowCreated;
            return Task.CompletedTask;
        }
    }
}

using System.Threading.Tasks;
using System.Windows.Media;
using ExplorerHub.Events;
using ExplorerHub.Framework;

namespace ExplorerHub.Infrastructure.BackgroundTasks
{
    /// <summary>
    /// 监测Win10系统主题颜色变化
    /// </summary>
    public class SystemUserPreferenceMonitoringTask : IBackgroundTask
    {
        private readonly IEventBus _eventBus;
        private readonly ISystemColorManager _colorManager;

        public SystemUserPreferenceMonitoringTask(IEventBus eventBus, ISystemColorManager colorManager)
        {
            _eventBus = eventBus;
            _colorManager = colorManager;
        }
        
        private void ColorManagerOnSystemColorChanged(object sender, Color newColor)
        {
            _eventBus.PublishEvent(new SystemColorChangedEventData(newColor));
        }
        
        public Task StartAsync()
        {
            _colorManager.SystemColorChanged += ColorManagerOnSystemColorChanged;
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _colorManager.SystemColorChanged -= ColorManagerOnSystemColorChanged;
            return Task.CompletedTask;
        }
    }
}

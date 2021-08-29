using System.Windows.Media;
using ExplorerHub.Events;

namespace ExplorerHub.BackgroundTasks
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

        public void Start()
        {
            _colorManager.SystemColorChanged += ColorManagerOnSystemColorChanged;
        }

        private void ColorManagerOnSystemColorChanged(object sender, Color newColor)
        {
            _eventBus.PublishEvent(new SystemColorChangedEventData(newColor));
        }

        public void Stop()
        {
            _colorManager.SystemColorChanged -= ColorManagerOnSystemColorChanged;
        }
    }
}

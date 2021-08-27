using System.Windows.Media;

namespace ExplorerHub.Events
{
    public class SystemColorChangedEventData : IEventData
    {
        public SystemColorChangedEventData(Color newColor)
        {
            NewColor = newColor;
        }

        public const string EventName = "SystemColorChanged";

        public string Name => EventName;
        
        public Color NewColor { get; }
    }
}

using ExplorerHub.Framework;

namespace ExplorerHub.ViewModels
{
    public class ExplorerCreatedEventData : IEventData
    {
        public ExplorerCreatedEventData(int explorerId)
        {
            ExplorerId = explorerId;
        }

        public const string EventName = "ExplorerCreated";

        public string Name => EventName;

        public int ExplorerId { get; }
    }
}

using ExplorerHub.Framework;

namespace ExplorerHub.ViewModels
{
    public class ExplorerNavigationUpdatedEventData : IEventData
    {
        public ExplorerNavigationUpdatedEventData(int explorerId)
        {
            ExplorerId = explorerId;
        }

        public const string EventName = "ExplorerNavigationUpdated";

        public string Name => EventName;

        public int ExplorerId { get; }
    }
}

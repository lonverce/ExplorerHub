using ExplorerHub.Framework;

namespace ExplorerHub.ViewModels
{
    public class ExplorerRemovedEventData : IEventData
    {
        public ExplorerRemovedEventData(int explorerId)
        {
            ExplorerId = explorerId;
        }

        public const string EventName = "ExplorerRemoved";

        public string Name => EventName;

        public int ExplorerId { get; }
    }
}
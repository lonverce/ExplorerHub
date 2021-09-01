using System;
using ExplorerHub.Framework;

namespace ExplorerHub.Domain.Favorites
{
    public class FavoriteAddedEventData : IEventData
    {
        public FavoriteAddedEventData(Guid newFavoriteId)
        {
            NewFavoriteId = newFavoriteId;
        }

        public const string EventName = "FavoriteAdded";

        public string Name => EventName;

        public Guid NewFavoriteId { get; }
    }
}

using System;
using ExplorerHub.Framework;

namespace ExplorerHub.Domain.Favorites
{
    public class FavoriteRemovedEventData : IEventData
    {
        public FavoriteRemovedEventData(Guid favoriteId)
        {
            FavoriteId = favoriteId;
        }

        public const string EventName = "FavoriteRemoved";

        public string Name => EventName;

        public Guid FavoriteId { get; }
    }
}
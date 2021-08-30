using System;
using System.Collections.Generic;

namespace ExplorerHub.Applications.Favorites
{
    public interface IFavoriteApplication
    {
        FavoriteDto AddFavorite(AddFavoriteRequest request);

        void DeleteFavorite(Guid favoriteId);

        List<FavoriteDto> GetAllFavorites();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExplorerHub.Applications.Favorites
{
    public interface IFavoriteApplication
    {
        Task<FavoriteDto> AddFavoriteAsync(AddFavoriteRequest request);

        Task DeleteFavoriteAsync(Guid favoriteId);

        Task<List<FavoriteDto>> GetAllFavoritesAsync();

        Task<FavoriteDto> FindAsync(Guid id);
    }
}

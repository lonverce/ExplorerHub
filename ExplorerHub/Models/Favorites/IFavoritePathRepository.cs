using System;

namespace ExplorerHub.Models.Favorites
{
    public interface IFavoritePathRepository
    {
        FavoritePath[] GetAllFavoritePath();

        void Add(FavoritePath path);

        void Update(FavoritePath path);

        void Delete(Guid id);
    }
}
using System;
using System.Collections.Generic;

namespace ExplorerHub.Domain.Favorites
{
    public interface IFavoriteRepository
    {
        Favorite Add(Favorite favorite);

        void Delete(Favorite favorite);

        Favorite FindById(Guid id);

        List<Favorite> GetAll();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ExplorerHub.Domain.Favorites;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore.Favorites
{
    public class FavoriteRepository : AbstractRepository, IFavoriteRepository
    {
        public Favorite Add(Favorite favorite)
        {
            favorite = DbContext.Favorites.Add(favorite).Entity;
            favorite.AddDomainEvent(new FavoriteAddedEventData(favorite.Id));
            return favorite;
        }

        public void Delete(Favorite favorite)
        {
            DbContext.Favorites.Remove(favorite);
            favorite.AddDomainEvent(new FavoriteRemovedEventData(favorite.Id));
        }

        public Favorite FindById(Guid id)
        {
            return DbContext.Favorites.AsTracking().FirstOrDefault(favorite => favorite.Id == id);
        }

        public List<Favorite> GetAll()
        {
            return DbContext.Favorites.AsTracking().ToList();
        }

        public FavoriteRepository(ExplorerHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}

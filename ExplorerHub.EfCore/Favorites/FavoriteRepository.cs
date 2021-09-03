using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorerHub.Domain.Favorites;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore.Favorites
{
    public class FavoriteRepository : AbstractRepository, IFavoriteRepository
    {
        public async Task<Favorite> AddAsync(Favorite favorite)
        {
            var entry = await DbContext.Favorites.AddAsync(favorite);
            favorite = entry.Entity;
            favorite.AddDomainEvent(new FavoriteAddedEventData(favorite.Id));
            return favorite;
        }

        public Task DeleteAsync(Favorite favorite)
        {
            Delete(favorite);
            return Task.CompletedTask;
        }

        public async Task<Favorite> FindByIdAsync(Guid id)
        {
            return await DbContext.Favorites.AsTracking()
                .FirstOrDefaultAsync(favorite => favorite.Id == id);
        }

        public async Task<List<Favorite>> GetAllAsync()
        {
            return await DbContext.Favorites.AsTracking().ToListAsync();
        }

        public void Delete(Favorite favorite)
        {
            DbContext.Favorites.Remove(favorite);
            favorite.AddDomainEvent(new FavoriteRemovedEventData(favorite.Id));
        }
        
        public FavoriteRepository(ExplorerHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}

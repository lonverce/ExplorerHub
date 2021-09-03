using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExplorerHub.Domain.Favorites
{
    public interface IFavoriteRepository
    {
        Task<Favorite> AddAsync(Favorite favorite);

        Task DeleteAsync(Favorite favorite);

        Task<Favorite> FindByIdAsync(Guid id);

        Task<List<Favorite>> GetAllAsync();
    }
}
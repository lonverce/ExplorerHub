using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.Framework.Domain;

namespace ExplorerHub.Applications.Favorites
{
    public class FavoriteApplication : IFavoriteApplication, IApplicationService
    {
        private readonly IFavoriteRepository _favorites;
        private readonly IMapper _mapper;

        public FavoriteApplication(IFavoriteRepository favorites, IMapper mapper)
        {
            _favorites = favorites;
            _mapper = mapper;
        }
        
        public async Task<FavoriteDto> AddFavoriteAsync(AddFavoriteRequest request)
        {
            var newFavorite = new Favorite(request.Name, request.Url, request.Icon);
            return _mapper.Map<FavoriteDto>( await _favorites.AddAsync(newFavorite));
        }

        public async Task DeleteFavoriteAsync(Guid favoriteId)
        {
            var favorite = await _favorites.FindByIdAsync(favoriteId);
            if (favorite == null)
            {
                return;
            }

            await _favorites.DeleteAsync(favorite);
        }

        public async Task<List<FavoriteDto>> GetAllFavoritesAsync()
        {
            return _mapper.Map<List<FavoriteDto>>(await _favorites.GetAllAsync());
        }

        public async Task<FavoriteDto> FindAsync(Guid id)
        {
            var favorite = await _favorites.FindByIdAsync(id);
            if (favorite == null)
            {
                return null;
            }

            return _mapper.Map<FavoriteDto>(favorite);
        }
    }
}

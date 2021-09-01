using System;
using System.Collections.Generic;
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

        public FavoriteDto AddFavorite(AddFavoriteRequest request)
        {
            var newFavorite = new Favorite(request.Name, request.Url, request.Icon);
            return _mapper.Map<FavoriteDto>(_favorites.Add(newFavorite));
        }

        public void DeleteFavorite(Guid favoriteId)
        {
            var favorite = _favorites.FindById(favoriteId);
            if (favorite == null)
            {
                return;
            }

            _favorites.Delete(favorite);
        }

        public List<FavoriteDto> GetAllFavorites()
        {
            return _mapper.Map<List<FavoriteDto>>(_favorites.GetAll());
        }

        public FavoriteDto Find(Guid id)
        {
            var favorite = _favorites.FindById(id);
            if (favorite == null)
            {
                return null;
            }

            return _mapper.Map<FavoriteDto>(favorite);
        }
    }
}

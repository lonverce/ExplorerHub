using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac.Features.OwnedInstances;
using ExplorerHub.Applications.Favorites;

namespace ExplorerHub.ViewModels.Favorites
{
    public class FavoriteViewModelProvider
    {
        private readonly FavoriteViewModel.ConstructFunc _factory;
        private readonly Dictionary<Guid, Owned<FavoriteViewModel>> _owneds;

        public ObservableCollection<FavoriteViewModel> Favorites { get; } =
            new ObservableCollection<FavoriteViewModel>();

        public FavoriteViewModelProvider(FavoriteViewModel.ConstructFunc factory)
        {
            _factory = factory;
            _owneds = new Dictionary<Guid, Owned<FavoriteViewModel>>();
        }

        public FavoriteViewModel Add(FavoriteDto favoriteDto)
        {
            if (_owneds.TryGetValue(favoriteDto.Id, out var favorite))
            {
                return favorite.Value;
            }

            favorite = _factory(favoriteDto);
            _owneds.Add(favorite.Value.Id, favorite);
            Favorites.Add(favorite.Value);
            return favorite.Value;
        }

        public bool Remove(Guid id)
        {
            if (!_owneds.TryGetValue(id, out var favorite))
            {
                return false;
            }

            var vm = favorite.Value;
            var result = Favorites.Remove(vm);
            favorite.Dispose();
            return result;
        }
    }
}

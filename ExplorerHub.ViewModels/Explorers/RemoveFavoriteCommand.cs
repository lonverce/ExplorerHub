using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.Explorers
{
    public class RemoveFavoriteCommand : AsyncCommand
    {
        private readonly ExplorerViewModel _vm;
        private readonly IFavoriteApplication _favoriteApplication;
        private readonly FavoriteViewModelProvider _favoriteViewModelProvider;

        public RemoveFavoriteCommand(ExplorerViewModel vm, IFavoriteApplication favoriteApplication, FavoriteViewModelProvider favoriteViewModelProvider)
        {
            _vm = vm;
            _favoriteApplication = favoriteApplication;
            _favoriteViewModelProvider = favoriteViewModelProvider;
            vm.PropertyChanged += VmOnPropertyChanged;
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_vm.IsCurrentNavigationInFavorite))
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override bool CanExecute(object parameter) => _vm.IsCurrentNavigationInFavorite;

        public override async Task ExecuteAsync(object parameter)
        {
            var model = _favoriteViewModelProvider.Favorites.Single(model => model.LocationUrl == _vm.NavigationPath);
            await _favoriteApplication.DeleteFavoriteAsync(model.Id);
        }

        public override event EventHandler CanExecuteChanged;
    }
}
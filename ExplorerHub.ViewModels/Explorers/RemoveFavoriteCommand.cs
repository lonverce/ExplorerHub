using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.Explorers
{
    public class RemoveFavoriteCommand : ICommand
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

        public bool CanExecute(object parameter) => _vm.IsCurrentNavigationInFavorite;

        public virtual void Execute(object parameter)
        {
            var model = _favoriteViewModelProvider.Favorites.Single(model => model.LocationUrl == _vm.NavigationPath);
            _favoriteApplication.DeleteFavorite(model.Id);
        }

        public event EventHandler CanExecuteChanged;
    }
}
using System;
using System.Windows.Input;
using ExplorerHub.Applications.Favorites;

namespace ExplorerHub.ViewModels.Favorites
{
    public class RemoveFavoriteLinkCommand : ICommand
    {
        private readonly FavoriteViewModel _vm;
        private readonly IFavoriteApplication _favoriteApplication;

        public RemoveFavoriteLinkCommand(FavoriteViewModel vm, IFavoriteApplication favoriteApplication)
        {
            _vm = vm;
            _favoriteApplication = favoriteApplication;
        }

        public bool CanExecute(object parameter) => true;

        public virtual void Execute(object parameter)
        {
            Execute();
        }

        public void Execute()
        {
            _favoriteApplication.DeleteFavorite(_vm.Id);
        }

        public event EventHandler CanExecuteChanged;
    }
}
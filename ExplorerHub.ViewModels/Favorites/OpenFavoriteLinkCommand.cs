using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ExplorerHub.ViewModels.Explorers;

namespace ExplorerHub.ViewModels.Favorites
{
    public class OpenFavoriteLinkCommand : ICommand
    {
        private readonly FavoriteViewModel _vm;

        public OpenFavoriteLinkCommand(FavoriteViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter) => true;

        public virtual void Execute(object parameter)
        {
            if (parameter is ExplorerViewModel explorer)
            {
                Execute(explorer);
            }
        }

        public void Execute(ExplorerViewModel explorer)
        {
            if (!Directory.Exists(_vm.LocationUrl))
            {
                var res = MessageBox.Show("此书签指向的路径不存在, 是否移除此书签?", "错误", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (res != MessageBoxResult.Yes)
                {
                    return;
                }

                _vm.RemoveFavoriteLink.Execute();
                return;
            }

            explorer.Search.Execute(_vm.LocationUrl);
        }

        public event EventHandler CanExecuteChanged;
    }
}
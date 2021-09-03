using System.IO;
using System.Threading.Tasks;
using System.Windows;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels.Explorers;

namespace ExplorerHub.ViewModels.Favorites
{
    public class OpenFavoriteLinkCommand : AsyncCommand
    {
        private readonly FavoriteViewModel _vm;

        public OpenFavoriteLinkCommand(FavoriteViewModel vm)
        {
            _vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (parameter is ExplorerViewModel explorer)
            {
                await ExecuteAsync(explorer);
            }
        }

        public async Task ExecuteAsync(ExplorerViewModel explorer)
        {
            if (!Directory.Exists(_vm.LocationUrl))
            {
                var res = MessageBox.Show("此书签指向的路径不存在, 是否移除此书签?", "错误", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (res != MessageBoxResult.Yes)
                {
                    return;
                }

                await _vm.RemoveFavoriteLink.ExecuteAsync();
                return;
            }

            await explorer.Search.ExecuteAsync(_vm.LocationUrl);
        }
    }
}
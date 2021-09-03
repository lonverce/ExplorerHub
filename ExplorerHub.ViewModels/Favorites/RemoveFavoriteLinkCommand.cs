using System.Threading.Tasks;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Favorites
{
    public class RemoveFavoriteLinkCommand : AsyncCommand
    {
        private readonly FavoriteViewModel _vm;
        private readonly IFavoriteApplication _favoriteApplication;

        public RemoveFavoriteLinkCommand(FavoriteViewModel vm, IFavoriteApplication favoriteApplication)
        {
            _vm = vm;
            _favoriteApplication = favoriteApplication;
        }
        
        public override async Task ExecuteAsync(object parameter)
        {
            await ExecuteAsync();
        }

        public async Task ExecuteAsync()
        {
            await _favoriteApplication.DeleteFavoriteAsync(_vm.Id);
        }
    }
}
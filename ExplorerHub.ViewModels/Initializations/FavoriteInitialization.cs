using System.Threading.Tasks;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Framework;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.Initializations
{
    public class FavoriteInitialization : IAppInitialization
    {
        private readonly FavoriteViewModelProvider _favoriteViewModelProvider;
        private readonly IFavoriteApplication _favoriteApplication;

        public FavoriteInitialization(FavoriteViewModelProvider favoriteViewModelProvider, IFavoriteApplication favoriteApplication)
        {
            _favoriteViewModelProvider = favoriteViewModelProvider;
            _favoriteApplication = favoriteApplication;
        }
        
        public async Task InitializeAppComponentsAsync()
        {
            foreach (var favoriteDto in await _favoriteApplication.GetAllFavoritesAsync())
            {
                _favoriteViewModelProvider.Add(favoriteDto);
            }
        }

        public Task ReleaseAppComponentAsync()
        {
            return Task.CompletedTask;
        }
    }
}

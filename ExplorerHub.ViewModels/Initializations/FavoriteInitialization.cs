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

        public void InitializeAppComponents()
        {
            foreach (var favoriteDto in _favoriteApplication.GetAllFavorites())
            {
                _favoriteViewModelProvider.Add(favoriteDto);
            }
        }

        public void ReleaseAppComponent()
        {
        }
    }
}

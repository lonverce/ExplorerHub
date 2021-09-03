using System.Threading.Tasks;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.Framework;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.Subscribers
{
    [EventSubscriber(FavoriteAddedEventData.EventName, UiHandle = true)]
    public class FavoriteAddedEventSubscriber : IEventSubscriber
    {
        private readonly FavoriteViewModelProvider _provider;
        private readonly IFavoriteApplication _favoriteApplication;

        public FavoriteAddedEventSubscriber(FavoriteViewModelProvider provider, IFavoriteApplication favoriteApplication)
        {
            _provider = provider;
            _favoriteApplication = favoriteApplication;
        }

        public async Task HandleAsync(IEventData eventData)
        {
            var data = (FavoriteAddedEventData) eventData;
            var favorite = await _favoriteApplication.FindAsync(data.NewFavoriteId);
            if (favorite == null)
            {
                return;
            }

            _provider.Add(favorite);
        }
    }
}

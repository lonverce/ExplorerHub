using System.Runtime.CompilerServices;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Domain.Favorites;
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

        public void Handle(IEventData eventData)
        {
            var data = (FavoriteAddedEventData) eventData;
            var favorite = _favoriteApplication.Find(data.NewFavoriteId);
            if (favorite == null)
            {
                return;
            }

            _provider.Add(favorite);
        }
    }
}

using System.Threading.Tasks;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.Framework;
using ExplorerHub.ViewModels.Favorites;

namespace ExplorerHub.ViewModels.Subscribers
{
    [EventSubscriber(FavoriteRemovedEventData.EventName, UiHandle = true)]
    public class FavoriteRemovedEventSubscriber : IEventSubscriber
    {
        private readonly FavoriteViewModelProvider _provider;

        public FavoriteRemovedEventSubscriber(FavoriteViewModelProvider provider)
        {
            _provider = provider;
        }

        public async Task HandleAsync(IEventData eventData)
        {
            await Task.CompletedTask;
            var data = (FavoriteRemovedEventData) eventData;
            _provider.Remove(data.FavoriteId);
        }
    }
}
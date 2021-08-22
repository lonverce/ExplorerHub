using ExplorerHub.Events;
using ExplorerHub.ViewModels;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(NewExplorerEventData.EventName, UiHandle = true)]
    public class NewBrowserEventSubscriber : IEventSubscriber
    {
        private readonly IHubWindowsManager _windowsManager;

        public NewBrowserEventSubscriber(IHubWindowsManager windowsManager)
        {
            _windowsManager = windowsManager;
        }

        public void Handle(IEventData eventData)
        {
            var data = (NewExplorerEventData) eventData;
            _windowsManager.GetOrCreateActiveHubWindow().AddBrowserCommand.Execute(data.Target);
        }
    }
}

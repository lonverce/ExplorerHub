using System;
using ExplorerHub.Events;
using ExplorerHub.ViewModels.Explorers;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(ExplorerNavigationUpdatedEventData.EventName, UiHandle = false)]
    public class NavigationChangedEventSubscriber : IEventSubscriber
    {
        private readonly IManagedObjectRepository<ExplorerViewModel> _explorers;

        public NavigationChangedEventSubscriber(IManagedObjectRepository<ExplorerViewModel> explorers)
        {
            _explorers = explorers;
        }
        
        public void Handle(IEventData eventData)
        {
            var data = (ExplorerNavigationUpdatedEventData) eventData;
            if (!_explorers.TryGetModelById(data.ExplorerId, out var model))
            {
                return;
            }

            var location = model.Browser.NavigationLog.CurrentLocation;
            if (!location.IsFileSystemObject)
            {
                Console.WriteLine($"non file-system object: {location.ParsingName}");
            }

            if (location.IsLink)
            {
                ShellLink link = (ShellLink)location;
                Console.WriteLine($"link object: {location.ParsingName}; target: {link.TargetShellObject.ParsingName}");
            }
        }
    }
}

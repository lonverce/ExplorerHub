using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.ViewModels
{
    public interface IHubWindowsManager
    {
        ExplorerHubViewModel CreateHubWindow();

        ExplorerHubViewModel GetOrCreateActiveHubWindow();
    }
}
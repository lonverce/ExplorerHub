namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public interface IHubWindowsManager
    {
        ExplorerHubViewModel CreateHubWindow();

        ExplorerHubViewModel GetOrCreateActiveHubWindow();
    }
}
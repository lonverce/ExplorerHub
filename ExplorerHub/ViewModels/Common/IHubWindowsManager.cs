namespace ExplorerHub.ViewModels
{
    public interface IHubWindowsManager
    {
        ExplorerHubViewModel CreateHubWindow();

        ExplorerHubViewModel GetOrCreateActiveHubWindow();
    }
}
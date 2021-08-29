using System.Linq;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.Infrastructures
{
    public class HubWindowsManager : IHubWindowsManager
    {
        private readonly App _app;
        private readonly IViewModelRepository<ExplorerHubViewModel> _hubRepository;

        public HubWindowsManager(
            App app,
            IViewModelRepository<ExplorerHubViewModel> hubRepository)
        {
            _app = app;
            _hubRepository = hubRepository;
        }
        
        public ExplorerHubViewModel CreateHubWindow()
        {
            var vm = _hubRepository.Create();
            var wnd = new ExplorerHubWindow(vm);
            wnd.Closed += (sender, args) => _hubRepository.Delete(vm.ManagedObjectId);
            wnd.Show();
            return vm;
        }

        public ExplorerHubViewModel GetOrCreateActiveHubWindow()
        {
            var hubWindows = _app.HubWindows.ToArray();
            if (!hubWindows.Any())
            {
                return CreateHubWindow();
            }

            var hubWnd = hubWindows.SingleOrDefault(window => window.IsActive);
            if (hubWnd == null)
            {
                hubWnd = hubWindows[0];
                hubWnd.ActivateEx();
            }
            
            return hubWnd.ViewModel;
        }
    }
}

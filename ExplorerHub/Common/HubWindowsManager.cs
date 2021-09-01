using System.Linq;
using ExplorerHub.Framework.WPF;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub
{
    public class HubWindowsManager : IHubWindowsManager
    {
        private readonly App _app;
        private readonly IManagedObjectRepository<ExplorerHubViewModel> _hubRepository;

        public HubWindowsManager(
            App app,
            IManagedObjectRepository<ExplorerHubViewModel> hubRepository)
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

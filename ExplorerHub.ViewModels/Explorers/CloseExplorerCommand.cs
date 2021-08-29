using System;
using System.Windows.Input;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub.ViewModels.Explorers
{
    public class CloseExplorerCommand : ICommand
    {
        private readonly IManagedObjectRepository<ExplorerHubViewModel> _hubs;
        private readonly ExplorerViewModel _vm;

        public CloseExplorerCommand(IManagedObjectRepository<ExplorerHubViewModel> hubs, ExplorerViewModel vm)
        {
            _hubs = hubs;
            _vm = vm;
        }

        bool ICommand.CanExecute(object parameter) => true;

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        public void Execute()
        {
            if (!_hubs.TryGetModelById(_vm.OwnerId, out var hubVm))
            {
                return;
            }
            hubVm.CloseBrowser.Execute(_vm);
        }

        public event EventHandler CanExecuteChanged;
    }
}
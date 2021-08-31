using System;
using System.Windows.Input;
using ExplorerHub.ViewModels.Explorers;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class CloseBrowserCommand : ICommand
    {
        private readonly IManagedObjectRepository<ExplorerViewModel> _explorerRepository;
        private readonly ExplorerHubViewModel _hubViewModel;

        public CloseBrowserCommand(
            IManagedObjectRepository<ExplorerViewModel> explorerRepository,
            ExplorerHubViewModel hubViewModel)
        {
            _explorerRepository = explorerRepository;
            _hubViewModel = hubViewModel;
        }

        bool ICommand.CanExecute(object parameter) => true;

        [Obsolete]
        public virtual void Execute(object parameter)
        {
            var explorerVm = (ExplorerViewModel)parameter;
            Execute(explorerVm, true);
        }

        public void Execute(ExplorerViewModel vm, bool releaseBrowser = true)
        {
            if (!_hubViewModel.Explorers.Contains(vm))
            {
                throw new InvalidOperationException();
            }

            var currentVm = _hubViewModel.SelectedIndex == -1 ? null : 
                _hubViewModel.Explorers[_hubViewModel.SelectedIndex];

            if (currentVm == vm)
            {
                if (_hubViewModel.SelectedIndex + 1 < _hubViewModel.Explorers.Count)
                {
                    _hubViewModel.SelectedIndex++;
                }
                else if (_hubViewModel.SelectedIndex > 0)
                {
                    _hubViewModel.SelectedIndex--;
                }
            }

            _hubViewModel.Explorers.Remove(vm);
            vm.OwnerId = -1;

            if (releaseBrowser)
            {
                _explorerRepository.Delete(vm.ManagedObjectId);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
﻿using System;
using System.Windows.Input;

namespace ExplorerHub.ViewModels
{
    public class CloseBrowserCommand : ICommand
    {
        private readonly IViewModelRepository<ExplorerViewModel> _explorerRepository;
        private readonly ExplorerHubViewModel _hubViewModel;

        public CloseBrowserCommand(
            IViewModelRepository<ExplorerViewModel> explorerRepository,
            ExplorerHubViewModel hubViewModel)
        {
            _explorerRepository = explorerRepository;
            _hubViewModel = hubViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
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
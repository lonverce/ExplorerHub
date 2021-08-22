using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels
{
    public class AddBrowserCommand : ICommand
    {
        private readonly IViewModelRepository<ExplorerViewModel> _explorerRepository;
        private readonly ExplorerHubViewModel _owner;
        private readonly IKnownFolderManager _folderManager;

        public AddBrowserCommand(
            IViewModelRepository<ExplorerViewModel> explorerRepository,
            ExplorerHubViewModel owner,
            IKnownFolderManager folderManager)
        {
            _explorerRepository = explorerRepository;
            _owner = owner;
            _folderManager = folderManager;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Execute(null);
        }

        public void Execute(ShellObject initialNav = null)
        {
            var vm = _explorerRepository.Create();
            vm.OwnerId = _owner.ManagedObjectId;
            vm.Browser.Navigate(initialNav ?? _folderManager.Default);

            _owner.Explorers.Add(vm);
            _owner.SelectedIndex = _owner.Explorers.Count - 1;
        }

        public void Execute(ExplorerViewModel model, int index)
        {
            model.OwnerId = _owner.ManagedObjectId;
            _owner.Explorers.Insert(index, model);
            _owner.SelectedIndex = _owner.Explorers.Count - 1;
        }

        public event EventHandler CanExecuteChanged;
    }
}
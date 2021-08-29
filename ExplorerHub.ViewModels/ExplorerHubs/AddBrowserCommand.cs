﻿using System;
using System.Windows.Input;
using ExplorerHub.ViewModels.Explorers;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class AddBrowserCommand : ICommand
    {
        private readonly IManagedObjectRepository<ExplorerViewModel> _explorerRepository;
        private readonly ExplorerHubViewModel _owner;
        private readonly IShellUrlParser _parser;

        public AddBrowserCommand(
            IManagedObjectRepository<ExplorerViewModel> explorerRepository,
            ExplorerHubViewModel owner,
            IShellUrlParser parser)
        {
            _explorerRepository = explorerRepository;
            _owner = owner;
            _parser = parser;
        }

        bool ICommand.CanExecute(object parameter) => true;

        void ICommand.Execute(object parameter)
        {
            Execute(null);
        }

        public void Execute(ShellObject initialNav = null)
        {
            var vm = _explorerRepository.Create();
            vm.OwnerId = _owner.ManagedObjectId;
            vm.Browser.Navigate(initialNav ?? _parser.Default);

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
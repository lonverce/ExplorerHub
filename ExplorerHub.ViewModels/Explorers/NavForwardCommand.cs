using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Controls;

namespace ExplorerHub.ViewModels.Explorers
{
    public class NavForwardCommand : ICommand
    {
        private readonly ExplorerViewModel _owner;

        public bool CanExecute { get; private set; } = false;

        public NavForwardCommand(ExplorerViewModel owner)
        {
            _owner = owner;
            
            _owner.Browser.NavigationLog.NavigationLogChanged += NavigationLogOnNavigationLogChanged;
            CanExecute = _owner.Browser.NavigationLog.CanNavigateForward;
        }

        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var canExec = _owner.Browser.NavigationLog.CanNavigateForward;

            if (canExec == CanExecute)
            {
                return;
            }
            CanExecute = canExec;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter) => CanExecute;

        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        public bool Execute()
        {
            if (!CanExecute)
            {
                return false;
            }

            _owner.Browser.NavigateLogLocation(NavigationLogDirection.Forward);
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
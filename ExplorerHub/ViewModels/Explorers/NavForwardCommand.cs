using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Controls;

namespace ExplorerHub.ViewModels.Explorers
{
    public class NavForwardCommand : ICommand
    {
        private readonly ExplorerViewModel _owner;
        private bool _canExec = false;

        public NavForwardCommand(ExplorerViewModel owner)
        {
            _owner = owner;
            
            _owner.Browser.NavigationLog.NavigationLogChanged += NavigationLogOnNavigationLogChanged;
            _canExec = _owner.Browser.NavigationLog.CanNavigateForward;
        }

        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var canExec = _owner.Browser.NavigationLog.CanNavigateForward;

            if (canExec == _canExec)
            {
                return;
            }
            _canExec = canExec;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public bool CanExecute(object parameter) => _canExec;

        public void Execute(object parameter)
        {
            Execute();
        }

        public bool Execute()
        {
            if (!_canExec)
            {
                return false;
            }

            _owner.Browser.NavigateLogLocation(NavigationLogDirection.Forward);
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
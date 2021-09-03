using System;
using System.Threading.Tasks;
using ExplorerHub.Framework.WPF;
using Microsoft.WindowsAPICodePack.Controls;

namespace ExplorerHub.ViewModels.Explorers
{
    public class NavBackCommand : AsyncCommand
    {
        private readonly ExplorerViewModel _owner;
        private bool _canExecute = false;

        public NavBackCommand(ExplorerViewModel owner)
        {
            _owner = owner;

            _owner.Browser.NavigationLog.NavigationLogChanged += NavigationLogOnNavigationLogChanged;
            _canExecute = _owner.Browser.NavigationLog.CanNavigateBackward;
        }

        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var canExec = _owner.Browser.NavigationLog.CanNavigateBackward;

            if (canExec == _canExecute)
            {
                return;
            }

            _canExecute = canExec;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public override bool CanExecute(object parameter) => _canExecute;

        public override async Task ExecuteAsync(object parameter)
        {
            await ExecuteAsync();   
        }

        public async Task<bool> ExecuteAsync()
        {
            await Task.CompletedTask;
            if (!_canExecute)
            {
                return false;
            }

            _owner.Browser.NavigateLogLocation(NavigationLogDirection.Backward);
            return true;
        }

        public override event EventHandler CanExecuteChanged;
    }
}
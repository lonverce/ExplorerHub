using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels
{
    public class GoToParentCommand:ICommand
    {
        private readonly ExplorerViewModel _owner;
        private bool _canExec = false;
        private ShellObject _parent;

        public GoToParentCommand(ExplorerViewModel owner)
        {
            _owner = owner;
            _owner.Browser.NavigationLog.NavigationLogChanged += NavigationLogOnNavigationLogChanged;
        }

        private void NavigationLogOnNavigationLogChanged(object sender, NavigationLogEventArgs e)
        {
            var log = (ExplorerBrowserNavigationLog)sender;
            var target = log.CurrentLocation;
            
            _parent = target.Parent;
            var canExec = _parent != null;
            if (canExec != _canExec)
            {
                _canExec = canExec;
                CanExecuteChanged?.Invoke(this, e);
            }
        }

        public bool CanExecute(object parameter) => _canExec;

        public void Execute(object parameter)
        {
            Execute();
        }

        public void Execute()
        {
            _owner.Browser.Navigate(_parent);
        }

        public event EventHandler CanExecuteChanged;
    }
}
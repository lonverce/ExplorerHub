using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    public class GoToParentCommand:ICommand
    {
        private readonly ExplorerViewModel _owner;
        private ShellObject _parent;

        public bool CanExecute { get; private set; } = false;

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
            if (canExec != CanExecute)
            {
                CanExecute = canExec;
                CanExecuteChanged?.Invoke(this, e);
            }
        }

        bool ICommand.CanExecute(object parameter) => CanExecute;

        void ICommand.Execute(object parameter)
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
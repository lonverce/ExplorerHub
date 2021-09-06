using ExplorerHub.Framework.WPF;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels.Explorers
{
    public class GoToParentCommand : SyncCommand
    {
        private readonly ExplorerViewModel _owner;
        private ShellObject _parent;

        private bool _canExecute;

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
            if (canExec != _canExecute)
            {
                _canExecute = canExec;
                OnCanExecuteChanged(e);
            }
        }

        public override bool CanExecute(object parameter) => _canExecute;

        public override void Execute(object parameter)
        {
            Execute();
        }

        public void Execute()
        {
            _owner.Browser.Navigate(_parent);
            
        }
    }
}
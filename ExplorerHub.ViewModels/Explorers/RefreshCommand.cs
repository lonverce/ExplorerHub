using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Explorers
{
    public class RefreshCommand : SyncCommand
    {
        private readonly ExplorerViewModel _owner;

        public RefreshCommand(ExplorerViewModel owner)
        {
            _owner = owner;
        }
        
        public override void Execute(object parameter)
        {
            Execute();
        }

        public void Execute()
        {
            _owner.Browser.NavigateLogLocation(_owner.Browser.NavigationLog.CurrentLocationIndex);
        }
    }
}
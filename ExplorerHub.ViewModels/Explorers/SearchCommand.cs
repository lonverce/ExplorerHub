using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Explorers
{
    public class SearchCommand : SyncCommand
    {
        private readonly ExplorerViewModel _owner;
        private readonly IShellUrlParser _parser;
        private readonly IUserNotificationService _notificationService;

        public SearchCommand(ExplorerViewModel owner, IShellUrlParser parser, IUserNotificationService notificationService)
        {
            _owner = owner;
            _parser = parser;
            _notificationService = notificationService;
        }
        
        public override void Execute(object parameter)
        {
            Execute(parameter.ToString());
        }

        public void Execute(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                _owner.FlushData();
                return;
            }

            if (!_parser.TryParse(address, out var target))
            {
                _notificationService.Notify("无法导航到指定路径", "操作失败", NotificationLevel.Warn, false);
                _owner.FlushData();
                return;
            }
            
            _owner.Browser.Navigate(target);
        }
    }
}
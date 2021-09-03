using System.Threading.Tasks;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.ViewModels.Explorers
{
    public class SearchCommand : AsyncCommand
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
        
        public override async Task ExecuteAsync(object parameter)
        {
            var address = (string) parameter;
            await ExecuteAsync(address:address);
        }

        public async Task ExecuteAsync(string address)
        {
            await Task.CompletedTask;
            if (string.IsNullOrWhiteSpace(address))
            {
                _owner.FlushData();
                return;
            }

            if (!_parser.TryParse(address, out var target))
            {
                await _notificationService.NotifyAsync("无法导航到指定路径", "操作失败", NotificationLevel.Warn, false);
                _owner.FlushData();
                return;
            }
            
            _owner.Browser.Navigate(target);
        }
    }
}
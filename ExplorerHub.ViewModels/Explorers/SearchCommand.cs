using System;
using System.Windows.Input;

namespace ExplorerHub.ViewModels.Explorers
{
    public class SearchCommand : ICommand
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

        bool ICommand.CanExecute(object parameter) => true;

        [Obsolete]
        public virtual void Execute(object parameter)
        {
            var address = (string) parameter;
            Execute(address:address);
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

        public event EventHandler CanExecuteChanged;
    }
}
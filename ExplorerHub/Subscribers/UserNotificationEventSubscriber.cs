﻿using System.Threading.Tasks;
using System.Windows.Forms;
using ExplorerHub.Framework;
using ExplorerHub.Framework.WPF;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(UserNotificationEventData.EventName, UiHandle = true)]
    public class UserNotificationEventSubscriber : IEventSubscriber
    {
        private readonly HiddenMainWindow _window;

        public UserNotificationEventSubscriber(App app)
        {
            _window = app.MainWindow as HiddenMainWindow;
        }

        public async Task HandleAsync(IEventData eventData)
        {
            await Task.CompletedTask;
            var data = (UserNotificationEventData) eventData;
            if (data.IsAsync)
            {
                var icon = data.Level switch
                {
                    NotificationLevel.Info => ToolTipIcon.Info,
                    NotificationLevel.Error => ToolTipIcon.Error,
                    NotificationLevel.Warn => ToolTipIcon.Warning,
                    _ => ToolTipIcon.None
                };

                _window.ShowUserMessage(data.Message, data.Title, icon);
            }
            else
            {
                var icon = data.Level switch
                {
                    NotificationLevel.Info => MessageBoxIcon.Information,
                    NotificationLevel.Error => MessageBoxIcon.Error,
                    NotificationLevel.Warn => MessageBoxIcon.Warning,
                    _ => MessageBoxIcon.None
                };

                MessageBox.Show(data.Message, data.Title, MessageBoxButtons.OK, icon);
            }
        }
    }
}

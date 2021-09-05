using System.Threading.Tasks;
using ExplorerHub.Framework;
using ExplorerHub.Framework.Events;
using ExplorerHub.Infrastructure;

namespace ExplorerHub.Subscribers
{
    [EventSubscriber(LogEventData.EventName, UiHandle = false)]
    internal class LogEventSubscriber : IEventSubscriber
    {
        private readonly ILogService _logService;

        public LogEventSubscriber(ILogService logService)
        {
            _logService = logService;
        }

        public Task HandleAsync(IEventData eventData)
        {
            _logService.Log((LogEventData)eventData);
            return Task.CompletedTask;
        }
    }
}

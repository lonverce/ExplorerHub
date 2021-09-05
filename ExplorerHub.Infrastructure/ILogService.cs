using ExplorerHub.Framework.Events;

namespace ExplorerHub.Infrastructure
{
    public interface ILogService
    {
        void Log(LogEventData logEventData);
    }
}
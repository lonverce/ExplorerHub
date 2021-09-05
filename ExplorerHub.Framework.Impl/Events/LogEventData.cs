using System;

namespace ExplorerHub.Framework.Events
{
    public class LogEventData : IEventData
    {
        public LogEventData(Type sourceType, string sourceMemberName, LogLevel level, string message, Exception exception, DateTime time, string threadName)
        {
            SourceType = sourceType;
            SourceMemberName = sourceMemberName;
            Level = level;
            Message = message;
            Exception = exception;
            Time = time;
            ThreadName = threadName;
        }

        public const string EventName = "Framework.Log";

        public string Name => EventName;

        public Type SourceType { get; }

        public string SourceMemberName { get; }

        public LogLevel Level { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public DateTime Time { get; }

        public string ThreadName { get; }
    }
}

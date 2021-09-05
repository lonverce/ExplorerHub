using System;
using System.Threading;
using ExplorerHub.Framework.Events;

namespace ExplorerHub.Framework
{
    internal class Logger<T> : ILogger<T>
    {
        private readonly IEventBus _eventBus;

        public Logger(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Log(LogLevel level, string message, Exception e = null, string memberName = null)
        {
            var thread = Thread.CurrentThread;
            _eventBus.PublishEvent(new LogEventData(typeof(T), memberName,
                level, message, e,
                DateTime.Now, 
                string.IsNullOrEmpty(thread.Name)? thread.ManagedThreadId.ToString() : thread.Name));
        }
    }
}

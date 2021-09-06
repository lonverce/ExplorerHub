using System;
using System.IO;
using System.Text;
using ExplorerHub.Framework;
using ExplorerHub.Framework.Events;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace ExplorerHub.Infrastructure
{
    internal class Log4NetLogService : ILogService
    {
        private readonly ILogger _logger;

        public Log4NetLogService()
        {
            var repo = LogManager.CreateRepository("explorer-hub");
            repo.Threshold = Level.Debug;
            var layout = new PatternLayout("%date [%-5level] [%thread] %message%newline%exception");
            layout.ActivateOptions();
            
            var appender = new RollingFileAppender
            {
                AppendToFile = true,
                DatePattern = "yyyy-MM-dd HH:mm:ss.fff",
                Encoding = Encoding.UTF8,
                File = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                        Environment.SpecialFolderOption.Create),
                    "ExplorerHub", "logs", "app.log"),
                MaximumFileSize = "4MB",
                StaticLogFileName = false,
                MaxSizeRollBackups = 10,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                PreserveLogFileNameExtension = true,
                Layout = layout,
            };

            appender.ActivateOptions();
            
            BasicConfigurator.Configure(repo, appender);
            
            _logger = repo.GetLogger("default");
        }

        public void Log(LogEventData logEventData)
        {
            _logger.Log(new LoggingEvent(new LoggingEventData
            {
                TimeStampUtc = logEventData.Time.ToUniversalTime(),
                Level = ConvertLevel(logEventData.Level),
                Message = $"{logEventData.SourceType.FullName}::{logEventData.SourceMemberName} {logEventData.Message}",
                ExceptionString = logEventData.Exception?.ToString(),
                ThreadName = logEventData.ThreadName
            }));
        }

        private static Level ConvertLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => Level.Debug,
                LogLevel.Info => Level.Info,
                LogLevel.Warn => Level.Warn,
                LogLevel.Error => Level.Error,
                LogLevel.Fatal => Level.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
    }
}

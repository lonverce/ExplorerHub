using System;
using System.Runtime.CompilerServices;

namespace ExplorerHub.Framework
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public interface ILogger<T>
    {
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="e"></param>
        /// <param name="memberName"></param>
        void Log(LogLevel level, string message, Exception e = null, [CallerMemberName]string memberName = null);
    }
}
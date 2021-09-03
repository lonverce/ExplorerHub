using System;
using System.Threading.Tasks;

namespace ExplorerHub.Framework.WPF
{
    /// <summary>
    /// 异步命令接口
    /// </summary>
    public interface IAsyncCommand
    {
        bool IsExecuting { get; }

        bool CanExecute(object parameter);

        Task ExecuteAsync(object parameter);

        event EventHandler CanExecuteChanged;
    }
}
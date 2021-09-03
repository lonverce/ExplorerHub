using System;
using System.Windows.Input;

namespace ExplorerHub.Framework.WPF
{
    public interface IAsyncCommandAdapter : ICommand
    {
        IAsyncCommand InnerCommand { get; }

        bool IsExecuting { get; }
        
        event EventHandler<Exception> ExecutionCompleted;
    }

    public interface IAsyncCommandAdapter<out TAsyncCommand> : IAsyncCommandAdapter
        where TAsyncCommand : IAsyncCommand
    {
        new TAsyncCommand InnerCommand { get; }
    }
}
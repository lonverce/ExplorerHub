using System;
using System.Threading.Tasks;

namespace ExplorerHub.Framework.WPF.Impl
{
    public class AsyncCommandAdapter : IAsyncCommandAdapter
    {
        private bool _executing;
        private event EventHandler<Exception> _executionCompleted;
        
        public IAsyncCommand InnerCommand { get; }
        
        /// <summary>
        /// 是否在执行中
        /// </summary>
        public bool IsExecuting => _executing;

        public event EventHandler CanExecuteChanged;
        
        public event EventHandler<Exception> ExecutionCompleted
        {
            add
            {
                if (!_executing)
                {
                    return;
                }

                _executionCompleted += value;
            }
            remove => _executionCompleted -= value;
        }

        public AsyncCommandAdapter(IAsyncCommand asyncCommand)
        {
            InnerCommand = asyncCommand;
            InnerCommand.CanExecuteChanged += InnerCommandOnCanExecuteChanged;
        }

        private void InnerCommandOnCanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public bool CanExecute(object parameter)
        {
            return !_executing && InnerCommand.CanExecute(parameter);
        }

        public virtual async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        
        public async Task ExecuteAsync(object parameter)
        {
            if (_executing)
            {
                return;
            }

            var commandTask = InnerCommand.ExecuteAsync(parameter);

            if (commandTask.IsCompleted)
            {
                if (commandTask.Exception != null)
                {
                    throw commandTask.Exception;
                }

                return;
            }

            UpdateExecuteStatus(true);

            Exception e = null;

            try
            {
                await commandTask;
            }
            catch(Exception ex)
            {
                e = ex;
            }
            finally
            {
                UpdateExecuteStatus(false, e);
            }
        }

        private void UpdateExecuteStatus(bool executing, Exception e = null)
        {
            _executing = executing;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            if (!executing)
            {
                var handlers = _executionCompleted;
                _executionCompleted = null;
                handlers?.Invoke(this, e);
            }
        }
    }

    public class AsyncCommandAdapter<TAsyncCommand> : AsyncCommandAdapter, IAsyncCommandAdapter<TAsyncCommand>
        where TAsyncCommand : IAsyncCommand
    {
        public AsyncCommandAdapter(TAsyncCommand asyncCommand) : base(asyncCommand)
        {
            InnerCommand = asyncCommand;
        }

        public new TAsyncCommand InnerCommand { get; }
    }
}

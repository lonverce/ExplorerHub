using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ExplorerHub.Framework.WPF.Annotations;

namespace ExplorerHub.Framework.WPF
{
    public abstract class AsyncCommand : IAsyncCommand, ICommand, INotifyPropertyChanged
    {
        private bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                _isExecuting = value;
                OnPropertyChanged();
            }
        }

        #region ICommand

        [UserEntry]
        public virtual async void Execute(object parameter)
        {
            if (IsExecuting)
            {
                throw new InvalidOperationException("AsyncCommand 正在执行中, 不可重新执行");
            }

            var commandTask = ExecuteAsync(parameter);
            if (commandTask.IsCompleted)
            {
                if (commandTask.Exception != null)
                {
                    throw commandTask.Exception;
                }
                return;
            }

            EnterExecute();
            Exception ex = null;
            try
            {
                await commandTask;
            }
            catch (Exception e)
            {
                ex = e;
            }
            finally
            {
                ExitExecute(ex);
            }
        }

        private void EnterExecute()
        {
            IsExecuting = true;
            OnCanExecuteChanged(EventArgs.Empty);
        }

        private void ExitExecute(Exception e)
        {
            IsExecuting = false;
            OnCanExecuteChanged(EventArgs.Empty);
            var handler = ExecutionCompleted;
            ExecutionCompleted = null;
            handler?.Invoke(this, e);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return !IsExecuting && CanExecute(parameter);
        }

        #endregion

        private void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public virtual bool CanExecute(object parameter) => true;

        public abstract Task ExecuteAsync(object parameter);

        public virtual event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<Exception> ExecutionCompleted; 

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

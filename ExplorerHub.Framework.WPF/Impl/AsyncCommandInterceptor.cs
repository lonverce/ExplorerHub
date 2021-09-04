using System;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal class AsyncCommandInterceptor : CommandInterceptor
    {
        protected override void OnCommandExecute(IInvocation invocation)
        {
            if (!(invocation.Proxy is AsyncCommand command))
            {
                base.OnCommandExecute(invocation);
                return;
            }

            if (command.IsExecuting)
            {
                throw new InvalidOperationException("AsyncCommand is executing.");
            }

            base.OnCommandExecute(invocation);

            if (command.IsExecuting)
            {
                command.ExecutionCompleted += (sender, exception) =>
                {
                    if (exception != null)
                    {
                        HandleExecutionException(exception);
                    }
                };
            }
        }

        public AsyncCommandInterceptor(ICommandExceptionHandler exceptionHandler) 
            : base(exceptionHandler)
        {
        }
    }
}
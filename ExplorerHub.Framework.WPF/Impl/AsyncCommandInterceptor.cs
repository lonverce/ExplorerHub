using System;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal class AsyncCommandInterceptor : CommandInterceptor
    {
        public override void Intercept(IInvocation invocation)
        {
            var command = (AsyncCommand)invocation.Proxy;
            if (command.IsExecuting)
            {
                throw new InvalidOperationException();
            }

            base.Intercept(invocation);

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
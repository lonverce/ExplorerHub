using System;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal class CommandInterceptor : IInterceptor
    {
        private readonly ICommandExceptionHandler _exceptionHandler;

        public CommandInterceptor(ICommandExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public virtual void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                HandleExecutionException(e);
            }
        }

        protected virtual void HandleExecutionException(Exception e)
        {
            _exceptionHandler.HandleException(e);
        }
    }
}

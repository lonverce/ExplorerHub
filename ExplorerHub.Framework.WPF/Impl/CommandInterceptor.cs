using System;
using System.Collections.Concurrent;
using System.Reflection;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.WPF.Impl
{
    internal class CommandInterceptor : IInterceptor
    {
        private readonly ICommandExceptionHandler _exceptionHandler;

        private static readonly ConcurrentDictionary<MethodInfo, bool> _isCommandExecuteMethod
            = new ConcurrentDictionary<MethodInfo, bool>();

        public CommandInterceptor(ICommandExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        private static bool IsUserEntry(MethodInfo method)
        {
            return _isCommandExecuteMethod.GetOrAdd(method, info =>
            {
                var attr = method.GetCustomAttribute<UserEntryAttribute>(true);
                return attr != null;
            });
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsUserEntry(invocation.Method))
            {
                OnUserEntryExecute(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }

        protected virtual void OnUserEntryExecute(IInvocation invocation)
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

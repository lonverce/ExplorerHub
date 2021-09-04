using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
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

        private static bool IsCommandExecuteMethod(MethodInfo method)
        {
            if (method.Name != "Execute")
            {
                return false;
            }

            return _isCommandExecuteMethod.GetOrAdd(method, info =>
            {
                Debug.Assert(method.DeclaringType != null);

                var map = method.DeclaringType.GetInterfaceMap(typeof(ICommand));
                return map.TargetMethods.Contains(method);
            });
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsCommandExecuteMethod(invocation.Method))
            {
                OnCommandExecute(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }

        protected virtual void OnCommandExecute(IInvocation invocation)
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

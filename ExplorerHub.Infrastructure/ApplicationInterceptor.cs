using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using Castle.DynamicProxy;
using ExplorerHub.Applications;

namespace ExplorerHub.Infrastructure
{
    public class ApplicationInterceptor : IInterceptor
    {
        private readonly IReadOnlyCollection<Meta<Func<Owned<IApplicationService>>>> _appServices;

        public ApplicationInterceptor(IEnumerable<Meta<Func<Owned<IApplicationService>>>> appServices)
        {
            _appServices = appServices.ToArray();
        }

        public void Intercept(IInvocation invocation)
        {
            var applicationInterfaceType = invocation.Method.DeclaringType;
            var appFactory = _appServices.First(meta =>
            {
                var targetInterface = (Type) meta.Metadata[ApplicationInterfaceKey];
                return targetInterface == applicationInterfaceType;
            });

            using var appOwned = appFactory.Value();
            var app = appOwned.Value;
            invocation.ReturnValue = invocation.Method.Invoke(app, invocation.Arguments);
        }

        public const string ApplicationInterfaceKey = "ApplicationInterface";
    }
}

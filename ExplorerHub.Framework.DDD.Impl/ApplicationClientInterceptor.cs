﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using Castle.DynamicProxy;
using ExplorerHub.Framework.Domain;
using Newtonsoft.Json;

namespace ExplorerHub.Framework.DDD.Impl
{
    public class ApplicationClientInterceptor : AsyncInterceptorBase
    {
        private readonly IReadOnlyDictionary<Type, Func<Owned<IApplicationService>>> _appServices;

        public ApplicationClientInterceptor(IEnumerable<Meta<Func<Owned<IApplicationService>>>> appServices)
        {
            var serviceMeta = appServices.ToDictionary(
                meta => (Type)meta.Metadata[ApplicationInterfaceKey], 
                meta => meta.Value);
            _appServices = serviceMeta;
        }
        
        public const string ApplicationInterfaceKey = "ApplicationInterface";

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            var method = invocation.Method;
            var applicationInterfaceType = method.DeclaringType
                                           ?? throw new ArgumentException(
                                               $"The DeclaringType of Method was not found. Method: {invocation.Method.Name}");

            if (!_appServices.TryGetValue(applicationInterfaceType, out var appConstructor))
            {
                throw new InvalidOperationException($"{applicationInterfaceType.FullName} is not regarded as an application service.");
            }
            
            await Task.Run(async () =>
            {
#if DEBUG
                Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] begin call '{applicationInterfaceType.FullName}::{method.Name}'({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})"); 
#endif
                try
                {
                    await using var appOwned = appConstructor();
                    var app = appOwned.Value;

                    await (Task)method.Invoke(app, invocation.Arguments);
#if DEBUG
                    Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] end call '{applicationInterfaceType.FullName}::{method.Name}'({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})");
#endif
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] error " +
                                      $"'{applicationInterfaceType.FullName}::{method.Name}'" +
                                      $"({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})\n{JsonConvert.SerializeObject(e)}");
#endif
                    throw new ApplicationServiceException(applicationInterfaceType.Name, method.Name, e.Message);
                }
            });
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var method = invocation.Method;
            var applicationInterfaceType = method.DeclaringType
                                           ?? throw new ArgumentException(
                                               $"The DeclaringType of Method was not found. Method: {invocation.Method.Name}");

            if (!_appServices.TryGetValue(applicationInterfaceType, out var appConstructor))
            {
                throw new InvalidOperationException($"{applicationInterfaceType.FullName} is not regarded as an application service.");
            }
            
            return await Task.Run(async () =>
            {
#if DEBUG
                Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] begin call '{applicationInterfaceType.FullName}::{method.Name}'({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})");
#endif
                try
                {
                    await using var appOwned = appConstructor();
                    var app = appOwned.Value;
                    var apiResult = await (Task<TResult>)method.Invoke(app, invocation.Arguments);
#if DEBUG
                    Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] success " +
                                      $"'{applicationInterfaceType.FullName}::{method.Name}'" +
                                      $"({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})\n{JsonConvert.SerializeObject(apiResult)}");
#endif
                    return apiResult;
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine($"[{DateTime.Now:s}] [{Thread.CurrentThread.Name}] error " +
                                      $"'{applicationInterfaceType.FullName}::{method.Name}'" +
                                      $"({string.Join(", ", invocation.Arguments.Select(JsonConvert.SerializeObject))})\n{JsonConvert.SerializeObject(e)}");
#endif
                    throw new ApplicationServiceException(applicationInterfaceType.Name, method.Name, e.Message);
                }
            });
        }
    }
}

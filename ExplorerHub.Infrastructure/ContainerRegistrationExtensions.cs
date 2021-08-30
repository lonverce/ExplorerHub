using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;

namespace ExplorerHub.Infrastructure
{
    public static class ContainerRegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InjectProperties<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            var propCollection = typeof(TLimit).GetProperties(BindingFlags.CreateInstance | BindingFlags.SetProperty | BindingFlags.Public |
                                                              BindingFlags.Instance).Select(prop => new
            {
                prop,
                attr = prop.GetCustomAttribute<InjectPropertyAttribute>()
            }).Where(arg => arg.attr != null).ToArray();

            if (!propCollection.Any())
            {
                return builder;
            }

            foreach (var p in propCollection)
            {
                if (p.attr.ResolvedType == null)
                {
                    p.attr.ResolvedType = p.prop.PropertyType;
                }
            }
            
            return builder.OnActivated(args =>
            {
                var instance = args.Instance;
                var ctx = args.Context;

                foreach (var p in propCollection)
                {
                    if (!ctx.TryResolve(p.attr.ResolvedType, out var pVal))
                    {
                        continue;
                    }

                    p.prop.SetValue(instance, pVal);
                }
            });
        }

        public static IRegistrationBuilder<TBackgroundTask, ConcreteReflectionActivatorData, SingleRegistrationStyle> AddBackgroundTask<TBackgroundTask>(this ContainerBuilder builder)
            where TBackgroundTask : IBackgroundTask
        {
            return builder.RegisterType<TBackgroundTask>()
                .InstancePerOwned<IBackgroundTask>()
                .As<IBackgroundTask>();
        }

        public static void AddEventSubscriber<TSubscriber>(this ContainerBuilder builder)
            where TSubscriber : IEventSubscriber
        {
            var attr = typeof(TSubscriber).GetCustomAttribute<EventSubscriberAttribute>();
            if (attr == null)
            {
                throw new ArgumentException($"{typeof(TSubscriber).FullName} 未携带必须的{nameof(EventSubscriberAttribute)}");
            }

            builder.RegisterType<TSubscriber>()
                .WithMetadata(nameof(EventSubscriberAttribute), attr)
                .As<IEventSubscriber>()
                .InstancePerOwned<IEventSubscriber>();
        }

        public static IRegistrationBuilder<TInitialization, ConcreteReflectionActivatorData, SingleRegistrationStyle> AddAppInitialization<TInitialization>(this ContainerBuilder builder)
            where TInitialization : IAppInitialization
        {
            return builder.RegisterType<TInitialization>()
                .As<IAppInitialization>()
                .SingleInstance();
        }

        public static void AddCommand<TCommand>(this ContainerBuilder builder)
            where TCommand : ICommand
        {
            builder.RegisterType<CommandInterceptor>()
                .IfNotRegistered(typeof(CommandInterceptor));

            builder.RegisterType<TCommand>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(CommandInterceptor));
        }
    }
}
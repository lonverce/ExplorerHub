using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using ExplorerHub.Applications;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.EfCore.Favorites;

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

        public static void AddApplicationServices(this ContainerBuilder builder, string connectionStr)
        {
            builder.RegisterType<ApplicationInterceptor>();
            builder.RegisterType<ProxyGenerator>()
                .IfNotRegistered(typeof(ProxyGenerator));

            builder.Register(context =>
            {
                var interceptor = context.Resolve<ApplicationInterceptor>();
                var generator = context.Resolve<ProxyGenerator>();
                return generator.CreateInterfaceProxyWithoutTarget<IFavoriteApplication>(interceptor);
            });

            builder.RegisterType<FavoriteApplication>()
                .As<IApplicationService>()
                .WithMetadata(ApplicationInterceptor.ApplicationInterfaceKey, typeof(IFavoriteApplication))
                .InstancePerOwned<IApplicationService>();

            builder.Register(context => new FavoriteDbContext(connectionStr))
                .InjectProperties()
                .As<IFavoriteRepository>()
                .InstancePerOwned<IApplicationService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Favorite, FavoriteDto>()
                    .ForMember(dto => dto.Url, expression => expression.MapFrom(favorite => favorite.Location));
            });

            builder.RegisterInstance(config.CreateMapper());
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
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
            return builder.PropertiesAutowired(PropertySelector, true);
        }

        private class InjectPropertySelector : IPropertySelector
        {
            public bool InjectProperty(PropertyInfo propertyInfo, object instance)
            {
                var attr = propertyInfo.GetCustomAttribute<InjectPropertyAttribute>();
                return attr != null;
            }
        }

        private static readonly IPropertySelector PropertySelector = new InjectPropertySelector();

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
                .SingleInstance()
                .As<IAppInitialization>();
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

        public static void AddEntityMapper(this ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Favorite, FavoriteDto>()
                    .ForMember(dto => dto.Url, expression => expression.MapFrom(favorite => favorite.Location));
            });

            builder.RegisterInstance(config.CreateMapper());
        }

        public static void AddExplorerHubDbContext(this ContainerBuilder builder, string connectionStr)
        {
            builder.Register(context => new FavoriteDbContext(connectionStr))
                .InjectProperties()
                .AsSelf()
                .As<IFavoriteRepository>()
                .InstancePerLifetimeScope();
        }

        public static void AddApplicationService<TAppServiceInterface, TAppService>(this ContainerBuilder builder)
            where TAppServiceInterface: class
            where TAppService : class, TAppServiceInterface, IApplicationService
        {
            builder.RegisterType<ApplicationInterceptor>()
                .IfNotRegistered(typeof(ApplicationInterceptor));

            builder.RegisterType<ProxyGenerator>()
                .IfNotRegistered(typeof(ProxyGenerator));

            builder.Register(context =>
            {
                var interceptor = context.Resolve<ApplicationInterceptor>();
                var generator = context.Resolve<ProxyGenerator>();
                return generator.CreateInterfaceProxyWithoutTarget<TAppServiceInterface>(interceptor);
            }).IfNotRegistered(typeof(TAppServiceInterface));

            builder.RegisterType<TAppService>()
                .As<IApplicationService>()
                .WithMetadata(ApplicationInterceptor.ApplicationInterfaceKey, typeof(TAppServiceInterface))
                .IfNotRegistered(typeof(TAppService));
        }
    }
}
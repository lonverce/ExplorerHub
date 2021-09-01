using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;

namespace ExplorerHub.Framework
{
    public static class FrameworkComponentRegisterExtensions
    {
        public static IRegistrationBuilder<TInitialization, ConcreteReflectionActivatorData, SingleRegistrationStyle> AddAppInitialization<TInitialization>(this ContainerBuilder builder)
            where TInitialization : IAppInitialization
        {
            return builder.RegisterType<TInitialization>()
                .SingleInstance()
                .As<IAppInitialization>();
        }

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

    }
}
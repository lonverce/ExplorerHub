using System.Windows.Input;
using Autofac;
using Autofac.Extras.DynamicProxy;
using ExplorerHub.Framework.WPF.Impl;

namespace ExplorerHub.Framework.WPF
{
    public static class WpfFrameworkRegisterExtensions
    {
        public static void AddManagedObjectPool<TObject>(this ContainerBuilder builder)
            where TObject: class, IManagedObject
        {
            builder.RegisterType<ManagedObjectPool<TObject>>()
                .AsSelf()
                .As<IManagedObjectRepository<TObject>>()
                .SingleInstance();
        }

        public static void AddCommand<TCommand>(this ContainerBuilder builder)
            where TCommand : ICommand
        {
            builder.RegisterType<TCommand>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(CommandInterceptor));
        }
    }
}
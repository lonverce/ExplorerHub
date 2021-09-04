using Autofac;
using Castle.DynamicProxy;
using ExplorerHub.Framework.Domain;

namespace ExplorerHub.Framework.DDD.Impl
{
    public static class DomainFrameworkRegisterExtensions
    {
        public static void AddApplicationService<TAppServiceInterface, TAppService>(this ContainerBuilder builder)
            where TAppServiceInterface : class
            where TAppService : class, TAppServiceInterface, IApplicationService
        {
            builder.Register(context =>
            {
                var interceptor = context.Resolve<ApplicationClientInterceptor>();
                var generator = context.Resolve<ProxyGenerator>();
                return generator.CreateInterfaceProxyWithoutTarget<TAppServiceInterface>(interceptor.ToInterceptor());
            }).IfNotRegistered(typeof(TAppServiceInterface));

            builder.RegisterType<TAppService>()
                .As<IApplicationService>()
                .WithMetadata(ApplicationClientInterceptor.ApplicationInterfaceKey, typeof(TAppServiceInterface))
                .IfNotRegistered(typeof(TAppService));
        }
    }
}
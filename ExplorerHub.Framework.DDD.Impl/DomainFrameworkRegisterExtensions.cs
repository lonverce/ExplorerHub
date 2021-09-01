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
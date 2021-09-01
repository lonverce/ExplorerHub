using Autofac;
using Castle.DynamicProxy;

namespace ExplorerHub.Framework.DDD.Impl
{
    public class DomainFrameworkRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ApplicationInterceptor>()
                .IfNotRegistered(typeof(ApplicationInterceptor));

            builder.RegisterType<ProxyGenerator>()
                .IfNotRegistered(typeof(ProxyGenerator));
        }
    }
}

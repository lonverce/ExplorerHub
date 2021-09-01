using Autofac;
using ExplorerHub.Framework.WPF.Impl;

namespace ExplorerHub.Framework.WPF
{
    public class WpfFrameworkRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<UiDispatcher>().As<IUiDispatcher>();
            builder.RegisterType<UserNotificationService>()
                .As<IUserNotificationService>();
            builder.RegisterType<CommandInterceptor>()
                .IfNotRegistered(typeof(CommandInterceptor));
        }
    }
}

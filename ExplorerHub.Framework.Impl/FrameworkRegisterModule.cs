using Autofac;
using ExplorerHub.Framework.BackgroundTasks;
using ExplorerHub.Framework.Initializations;
using MindLab.Messaging;
using Module = Autofac.Module;

namespace ExplorerHub.Framework
{
    public class FrameworkRegisterModule : Module
    {
        private readonly IAppLeader _leader;

        public FrameworkRegisterModule(IAppLeader leader)
        {
            _leader = leader;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(context => new BroadcastMessageRouter<IEventData>())
                .As<IMessageRouter<IEventData>>()
                .As<IMessagePublisher<IEventData>>()
                .SingleInstance();

            containerBuilder.RegisterType<EventBus>()
                .SingleInstance()
                .As<IEventBus>();

            containerBuilder.AddAppInitialization<BackgroundTasksInitialization>();
            containerBuilder.AddBackgroundTask<EventMessageDispatchTask>();
            containerBuilder.AddBackgroundTask<FollowerProcessWatchingTask>()
                .WithParameter(new TypedParameter(typeof(IAppLeader), _leader));
        }
    }
}

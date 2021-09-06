using Autofac;
using ExplorerHub.Framework.BackgroundTasks;
using ExplorerHub.Framework.Initializations;
using MindLab.Threading;
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
            var queue = new AsyncBlockingCollection<IEventData>();
            containerBuilder.RegisterType<EventBus>()
                .SingleInstance()
                .WithParameter("queue", queue)
                .As<IEventBus>();
            containerBuilder.RegisterType<BackgroundTaskManager>().SingleInstance();
            containerBuilder.AddAppInitialization<BackgroundTasksInitialization>();
            containerBuilder.AddBackgroundTask<EventMessageDispatchTask>()
                .WithParameter("queue", queue);
            containerBuilder.AddBackgroundTask<FollowerProcessWatchingTask>()
                .WithParameter(new TypedParameter(typeof(IAppLeader), _leader));
            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));
            containerBuilder.RegisterType<AppExecutor>().SingleInstance();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using ExplorerHub.AppInitializations;
using ExplorerHub.BackgroundTasks;
using ExplorerHub.Infrastructures;
using ExplorerHub.Models.Favorites;
using ExplorerHub.Repositories;
using ExplorerHub.Subscribers;
using ExplorerHub.ViewModels;
using MindLab.Messaging;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ExplorerHub
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        #region Fields
        private IContainer _container;
        private IAppLeader _leader; 
        #endregion

        public StartupEventArgs StartupEventArgs { get; private set; }

        public IEnumerable<ExplorerHubWindow> HubWindows => Windows.OfType<ExplorerHubWindow>();
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
            StartupEventArgs = e;

            // 确保当前进程是单例
            if(!EnsureIamLeader()) return;
            
            // 注册所有组件
            OnConfigureServices();
            
            // 执行所有初始化
            foreach (var initialization in _container.Resolve<IEnumerable<IAppInitialization>>())
            {
                initialization.InitializeAppComponents();
            }
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), e.Exception.GetType().FullName);
            e.Handled = true;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Domain Error");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container?.Dispose();
            base.OnExit(e);
        }

        private bool EnsureIamLeader()
        {
            var elector = new AppElector();

            if (!elector.TryRunForLeader(out _leader))
            {
                elector.SendMessageToLeader(StartupEventArgs.Args);
                Shutdown(0);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 注册所有组件
        /// </summary>
        private void OnConfigureServices()
        {
            var containerBuilder = new ContainerBuilder();
            var splashScreen = new SplashScreen("Splash.png");
            splashScreen.Show(false);

            // app
            containerBuilder.RegisterInstance(this)
                .AsSelf()
                .As<Application>();

            // infrastructures 
            containerBuilder.RegisterType<ManagedObjectPool<ExplorerViewModel>>()
                .AsSelf()
                .As<IViewModelRepository<ExplorerViewModel>>()
                .SingleInstance();

            containerBuilder.RegisterType<ManagedObjectPool<ExplorerHubViewModel>>()
                .AsSelf()
                .As<IViewModelRepository<ExplorerHubViewModel>>()
                .SingleInstance();

            containerBuilder.RegisterType<HubWindowsManager>()
                .As<IHubWindowsManager>()
                .SingleInstance();

            containerBuilder.RegisterType<KnownFolderManager>()
                .As<IKnownFolderManager>()
                .SingleInstance();

            containerBuilder.Register(context => new BroadcastMessageRouter<IEventData>())
                .As<IMessageRouter<IEventData>>()
                .As<IMessagePublisher<IEventData>>()
                .SingleInstance();

            containerBuilder.RegisterType<EventBus>()
                .SingleInstance()
                .As<IEventBus>();
            containerBuilder.RegisterType<UserNotificationService>()
                .As<IUserNotificationService>();
            containerBuilder.RegisterType<ShellWindowManager>()
                .As<IShellWindowsManager>()
                .SingleInstance();

            containerBuilder.RegisterType<FavoritePathRepository>()
                .SingleInstance()
                .As<IFavoritePathRepository>();

            // initializations
            containerBuilder.AddAppInitialization<MainWindowInitialization>();
            containerBuilder.AddAppInitialization<StartupArgInitialization>()
                .WithParameter(new TypedParameter(typeof(SplashScreen), splashScreen));
            containerBuilder.AddAppInitialization<BackgroundTasksInitialization>();

            // background tasks
            containerBuilder.AddBackgroundTask<EventMessageDispatchTask>();
            containerBuilder.AddBackgroundTask<FollowerProcessWatchingTask>()
                .WithParameter(new TypedParameter(typeof(IAppLeader), _leader));
            containerBuilder.AddBackgroundTask<ExternalShellWindowsMonitoringTask>();
            
            // event subscribers
            containerBuilder.AddEventSubscriber<NewBrowserEventSubscriber>();
            containerBuilder.AddEventSubscriber<FollowerStartupEventSubscriber>();
            containerBuilder.AddEventSubscriber<UserNotificationEventSubscriber>();

            // view models
            containerBuilder.RegisterType<ExplorerHubViewModel>()
                .InjectProperties()
                .AsSelf()
                .InstancePerOwned<ExplorerHubViewModel>();

            containerBuilder.RegisterType<ExplorerViewModel>()
                .InjectProperties()
                .AsSelf()
                .InstancePerOwned<ExplorerViewModel>();

            // commands
            containerBuilder.RegisterType<AddBrowserCommand>();
            containerBuilder.RegisterType<SearchCommand>();
            containerBuilder.RegisterType<NavBackCommand>();
            containerBuilder.RegisterType<NavForwardCommand>();
            containerBuilder.RegisterType<GoToParentCommand>();
            containerBuilder.RegisterType<CloseBrowserCommand>();
            containerBuilder.RegisterType<ShowInNewWindowCommand>();
            containerBuilder.RegisterType<ExplorerHubDropTarget>();

            // done
            _container = containerBuilder.Build();
        }
    }
}

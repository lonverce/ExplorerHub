using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Autofac;
using ExplorerHub.AppInitializations;
using ExplorerHub.EfCore;
using ExplorerHub.Infrastructure;
using ExplorerHub.Infrastructure.BackgroundTasks;
using ExplorerHub.Infrastructure.Initializations;
using ExplorerHub.Subscribers;
using ExplorerHub.UI;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;
using ExplorerHub.ViewModels.Explorers;
using ExplorerHub.ViewModels.Subscribers;
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

        public void SetAppBackground(Color color)
        {
            var sameColor = color.RemoveAlpha();
            // 背景亮度
            var brightness = (sameColor.R * 19595 + sameColor.G * 38469 + sameColor.B * 7472) >> 16;

            // 背景灰度百分比
            var gray = (1 - brightness / 255f) * 100;
            var foregroundColor = gray < 50 ? Color.FromRgb(40,40,40) : Colors.White;

            Resources[AppColors.SystemBackgroundKey] = new SolidColorBrush(color);
            Resources[AppColors.SystemForegroundKey] = new SolidColorBrush(foregroundColor);

            sameColor.A = 180;
            var inactiveBg = sameColor.RemoveAlpha();
            Resources[AppColors.SystemInactiveBackgroundKey] = new SolidColorBrush(inactiveBg);
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
            containerBuilder.RegisterType<SystemColorManager>()
                .As<ISystemColorManager>()
                .SingleInstance();

            containerBuilder.RegisterType<ManagedObjectPool<ExplorerViewModel>>()
                .AsSelf()
                .As<IManagedObjectRepository<ExplorerViewModel>>()
                .SingleInstance();

            containerBuilder.RegisterType<ManagedObjectPool<ExplorerHubViewModel>>()
                .AsSelf()
                .As<IManagedObjectRepository<ExplorerHubViewModel>>()
                .SingleInstance();

            containerBuilder.RegisterType<HubWindowsManager>()
                .As<IHubWindowsManager>()
                .SingleInstance();

            containerBuilder.RegisterType<ShellUrlParser>()
                .As<IShellUrlParser>()
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

            // initializations
            containerBuilder.AddAppInitialization<DbContextInitialization>();
            containerBuilder.AddAppInitialization<MainWindowInitialization>();
            containerBuilder.AddAppInitialization<StartupArgInitialization>()
                .WithParameter(new TypedParameter(typeof(SplashScreen), splashScreen));
            containerBuilder.AddAppInitialization<BackgroundTasksInitialization>();

            // background tasks
            containerBuilder.AddBackgroundTask<EventMessageDispatchTask>();
            containerBuilder.AddBackgroundTask<FollowerProcessWatchingTask>()
                .WithParameter(new TypedParameter(typeof(IAppLeader), _leader));
            containerBuilder.AddBackgroundTask<ExternalShellWindowsMonitoringTask>();
            containerBuilder.AddBackgroundTask<SystemUserPreferenceMonitoringTask>();

            // event subscribers
            containerBuilder.AddEventSubscriber<NewBrowserEventSubscriber>();
            containerBuilder.AddEventSubscriber<FollowerStartupEventSubscriber>();
            containerBuilder.AddEventSubscriber<UserNotificationEventSubscriber>();
            containerBuilder.AddEventSubscriber<SystemColorEventSubscriber>();
#if DEBUG
            containerBuilder.AddEventSubscriber<NavigationChangedEventSubscriber>(); 
#endif

            // view models
            containerBuilder.RegisterType<ExplorerHubViewModel>()
                .InjectProperties()
                .AsSelf()
                .InstancePerOwned<ExplorerHubViewModel>();

            containerBuilder.RegisterType<ExplorerViewModel>()
                .InjectProperties()
                .AsSelf()
                .InstancePerOwned<ExplorerViewModel>();

            containerBuilder.RegisterType<FavoriteViewModel>()
                .InjectProperties()
                .AsSelf();

            // commands
            containerBuilder.AddCommand<AddBrowserCommand>();
            containerBuilder.AddCommand<SearchCommand>();
            containerBuilder.AddCommand<NavBackCommand>();
            containerBuilder.AddCommand<NavForwardCommand>();
            containerBuilder.AddCommand<GoToParentCommand>();
            containerBuilder.AddCommand<CloseBrowserCommand>();
            containerBuilder.AddCommand<ShowInNewWindowCommand>();
            containerBuilder.RegisterType<ExplorerHubDropTarget>();
            containerBuilder.AddCommand<CloseExplorerCommand>();

            // application services
            var appDataDir = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create);

            containerBuilder.AddApplicationServices(Path.Combine(appDataDir, "explorer-hub.db"));

            // done
            _container = containerBuilder.Build();
        }
    }
}

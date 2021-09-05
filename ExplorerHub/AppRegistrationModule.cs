using System;
using System.IO;
using System.Windows;
using Autofac;
using ExplorerHub.AppInitializations;
using ExplorerHub.Applications.Favorites;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.EfCore;
using ExplorerHub.EfCore.Favorites;
using ExplorerHub.Framework;
using ExplorerHub.Framework.DDD.Impl;
using ExplorerHub.Framework.WPF;
using ExplorerHub.Infrastructure;
using ExplorerHub.Infrastructure.BackgroundTasks;
using ExplorerHub.Subscribers;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;
using ExplorerHub.ViewModels.Explorers;
using ExplorerHub.ViewModels.Favorites;
using ExplorerHub.ViewModels.Initializations;
using ExplorerHub.ViewModels.Subscribers;

namespace ExplorerHub
{
    public class AppRegistrationModule : Module
    {
        private readonly App _app;
        private readonly SplashScreen _splashScreen;

        public AppRegistrationModule(App app, SplashScreen splashScreen)
        {
            _app = app;
            _splashScreen = splashScreen;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            // app
            containerBuilder.RegisterInstance(_app)
                .AsSelf()
                .As<Application>();

            // infrastructures 
            containerBuilder.AddManagedObjectPool<ExplorerViewModel>();
            containerBuilder.AddManagedObjectPool<ExplorerHubViewModel>();

            containerBuilder.RegisterType<SystemColorManager>()
                .As<ISystemColorManager>()
                .SingleInstance();
            
            containerBuilder.RegisterType<HubWindowsManager>()
                .As<IHubWindowsManager>()
                .SingleInstance();

            containerBuilder.RegisterType<ShellUrlParser>()
                .As<IShellUrlParser>()
                .SingleInstance();
            
            containerBuilder.RegisterType<ShellWindowManager>()
                .As<IShellWindowsManager>()
                .SingleInstance();

            containerBuilder.RegisterType<AbsorbService>().As<IAbsorbService>();
            containerBuilder.UseLog4NetLogService();

            // initializations
            containerBuilder.AddAppInitialization<MainWindowInitialization>();
            containerBuilder.AddAppInitialization<ExternalShellWindowInitialization>();
            containerBuilder.AddAppInitialization<StartupArgInitialization>()
                .WithParameter(new TypedParameter(typeof(SplashScreen), _splashScreen));
            containerBuilder.AddAppInitialization<DbContextInitialization>();
            containerBuilder.AddAppInitialization<FavoriteInitialization>();

            // background tasks
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
            containerBuilder.AddEventSubscriber<FavoriteAddedEventSubscriber>();
            containerBuilder.AddEventSubscriber<FavoriteRemovedEventSubscriber>();
            containerBuilder.AddEventSubscriber<LogEventSubscriber>();

            // view models
            containerBuilder.RegisterType<FavoriteViewModelProvider>()
                .SingleInstance();

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
                .InstancePerOwned<FavoriteViewModel>()
                .AsSelf();

            // commands
            containerBuilder.AddCommand<AddBrowserCommand>();
            containerBuilder.AddCommand<SearchCommand>();
            containerBuilder.AddCommand<NavBackCommand>();
            containerBuilder.AddCommand<NavForwardCommand>();
            containerBuilder.AddCommand<GoToParentCommand>();
            containerBuilder.AddCommand<CloseBrowserCommand>();
            containerBuilder.AddCommand<ShowInNewWindowCommand>();
            containerBuilder.AddCommand<CloseExplorerCommand>();

            containerBuilder.AddAsyncCommand<AddFavoriteCommand>();
            containerBuilder.AddAsyncCommand<RemoveFavoriteCommand>();
            containerBuilder.AddAsyncCommand<OpenFavoriteLinkCommand>();
            containerBuilder.AddAsyncCommand<RemoveFavoriteLinkCommand>();

            // others
            containerBuilder.RegisterType<ExplorerHubDropTarget>();

            // application services
            containerBuilder.AddApplicationService<IFavoriteApplication, FavoriteApplication>();

            // repositories
            containerBuilder.AddRepository<IFavoriteRepository, FavoriteRepository>();

            // database
            var appDataDir = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create), "ExplorerHub");

            Directory.CreateDirectory(appDataDir);

            containerBuilder.AddExplorerHubDbContext(Path.Combine(appDataDir, "explorer-hub.db"));

            // mapper
            containerBuilder.AddEntityMapper();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Autofac;
using ExplorerHub.Framework;
using ExplorerHub.Framework.DDD.Impl;
using ExplorerHub.Framework.WPF;
using ExplorerHub.Infrastructure;
using ExplorerHub.UI;
using MessageBox = System.Windows.MessageBox;

namespace ExplorerHub
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public AppStartupOptions Options { get; }

        #region Fields
        private IContainer _container;
        private IAppLeader _leader;
        private AppExecutor _appExecutor;
        #endregion

        public App(AppStartupOptions options = null)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public StartupEventArgs StartupEventArgs { get; private set; }

        public IEnumerable<ExplorerHubWindow> HubWindows => Windows.OfType<ExplorerHubWindow>();
        
        protected override async void OnStartup(StartupEventArgs e)
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
            _appExecutor = _container.Resolve<AppExecutor>();
            await _appExecutor.StartAsync();
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

        protected override async void OnExit(ExitEventArgs e)
        {
            await _appExecutor.StopAsync();
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

            containerBuilder.RegisterModule(new FrameworkRegisterModule(_leader));
            containerBuilder.RegisterModule(new DomainFrameworkRegisterModule());
            containerBuilder.RegisterModule(new WpfFrameworkRegisterModule());
            containerBuilder.RegisterModule(new AppRegistrationModule(this, splashScreen));

            // done
            _container = containerBuilder.Build();
        }
    }
}

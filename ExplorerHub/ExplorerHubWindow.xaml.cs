using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using ExplorerHub.ViewModels.ExplorerHubs;

namespace ExplorerHub
{
    /// <summary>
    /// ExplorerHubWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExplorerHubWindow
    {
        private readonly ExplorerHubViewModel _vm;

        public ExplorerHubViewModel ViewModel => _vm;

        public ExplorerHubWindow(ExplorerHubViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;

            InitializeComponent();
            Loaded += OnWindowLoaded;
        }
        
        public ExplorerHubWindow():this(null)
        {
        }

        protected override void OnClosed(EventArgs e)
        {
            var vm = _vm;
            vm.Explorers.CollectionChanged -= ExplorersOnCollectionChanged;

            if (vm.Explorers.Any())
            {
                foreach (var explorerViewModel in vm.Explorers.ToArray())
                {
                    vm.CloseBrowser.Execute(explorerViewModel);
                }
            }

            base.OnClosed(e);
        }

        /// <summary>
        /// 窗台加载后初始化数据模型
        /// </summary>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var vm = (ExplorerHubViewModel)DataContext;
            vm.Explorers.CollectionChanged += ExplorersOnCollectionChanged;
            // 如果窗体已经被最小化，那么，当程序后台捕捉到新窗体并插入TabControl后，
            // 需要把窗体激活出来让用户知道
            ExplorerHeaderBar.SelectionChanged += (o, args) =>
            {
                this.ActivateEx();
            };
        }

        private void ExplorersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var explorers = (ICollection) sender;
            if (explorers.Count == 0)
            {
                // 所有ExplorerBrowser关闭后退出本窗口
                Close();
            }
        }
    }
}

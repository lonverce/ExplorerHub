using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ExplorerHub.UI;
using ExplorerHub.ViewModels;
using ExplorerHub.ViewModels.ExplorerHubs;
using TextBox = System.Windows.Controls.TextBox;

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
                    vm.CloseBrowserCommand.Execute(explorerViewModel);
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
            SearchBox_OnLostKeyboardFocus(SearchBox, null);
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

        private void OnSearchBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (TextBox)sender;
            e.Handled = tb.Focus();
        }

        private void SearchBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.PreviewMouseDown += OnSearchBoxPreviewMouseDown;

            AddressBar.Background = Brushes.WhiteSmoke;
            AddressBar.BorderThickness = new Thickness(1);
            AddressBar.BorderBrush = Brushes.Transparent;
        }

        private void SearchBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.PreviewMouseDown -= OnSearchBoxPreviewMouseDown;

            AddressBar.Background = Brushes.White;
            AddressBar.BorderThickness = new Thickness(1);
            AddressBar.BorderBrush = Brushes.DeepSkyBlue;
            tb.SelectAll();
        }
    }
}

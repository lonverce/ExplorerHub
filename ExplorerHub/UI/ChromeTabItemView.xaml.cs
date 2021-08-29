using System.Windows;

namespace ExplorerHub.UI
{
    /// <summary>
    /// ChromeTabItemView.xaml 的交互逻辑
    /// </summary>
    public partial class ChromeTabItemView
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), 
            typeof(ChromeTabItemView), new PropertyMetadata(false, OnIsSelectedChanged));

        private ITabView _currentTabView;

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public double Threshold { get; set; } = 35;

        public ChromeTabItemView()
        {
            _currentTabView = new UnselectedTabView(this);
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }
        
        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabView = (ChromeTabItemView) d;
            tabView._currentTabView.Expand();

            if ((bool)e.NewValue)
            {
                tabView._currentTabView = new SelectedTabView(tabView);
            }
            else
            {
                tabView._currentTabView = new UnselectedTabView(tabView);
            }

            if (!tabView.IsNormalWidth(tabView.ActualWidth))
            {
                tabView._currentTabView.Collapse();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!e.WidthChanged)
            {
                return;
            }

            var isCurrentWidthNormal = IsNormalWidth(e.NewSize.Width);
            var isPreviousWidthNormal = IsNormalWidth(e.PreviousSize.Width);

            if (isCurrentWidthNormal == isPreviousWidthNormal)
            {
                return;
            }

            if (isPreviousWidthNormal)
            {
                _currentTabView.Collapse();
            }
            else
            {
                _currentTabView.Expand();
            }
        }

        private bool IsNormalWidth(double width)
        {
            return width > Threshold;
        }

        private interface ITabView
        {
            void Expand();

            void Collapse();
        }

        private sealed class SelectedTabView : ITabView
        {
            private readonly ChromeTabItemView _owner;

            public SelectedTabView(ChromeTabItemView owner)
            {
                _owner = owner;
            }

            public void Expand()
            {
                _owner.Logo.Visibility = Visibility.Visible;
            }

            public void Collapse()
            {
                _owner.Logo.Visibility = Visibility.Collapsed;
            }
        }

        private sealed class UnselectedTabView : ITabView
        {
            private readonly ChromeTabItemView _owner;

            public UnselectedTabView(ChromeTabItemView owner)
            {
                _owner = owner;
            }

            public void Expand()
            {
                _owner.CloseBtn.Visibility = Visibility.Visible;
            }

            public void Collapse()
            {
                _owner.CloseBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}

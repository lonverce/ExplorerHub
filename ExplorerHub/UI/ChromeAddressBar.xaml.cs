using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExplorerHub.UI
{
    /// <summary>
    /// ChromeAddressBar.xaml 的交互逻辑
    /// </summary>
    public partial class ChromeAddressBar : UserControl
    {
        public ChromeAddressBar()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLostKeyboardFocus(SearchBox, null);
        }

        private void OnSearchBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (TextBox)sender;
            e.Handled = tb.Focus();
        }
        
        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.PreviewMouseDown += OnSearchBoxPreviewMouseDown;

            AddressBar.Background = Brushes.WhiteSmoke;
            AddressBar.BorderBrush = Brushes.Transparent;
        }

        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.PreviewMouseDown -= OnSearchBoxPreviewMouseDown;

            AddressBar.Background = Brushes.White;
            AddressBar.BorderBrush = Brushes.DodgerBlue;
            tb.SelectAll();
        }
    }
}

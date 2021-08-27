using System.Windows;
using System.Windows.Controls;

namespace ExplorerHub.UI
{
    public class ChromeTabItem : ListBoxItem
    {
        static ChromeTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabItem), new FrameworkPropertyMetadata(typeof(ChromeTabItem)));
        }
    }
}

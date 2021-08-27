using System.Windows;
using System.Windows.Controls;

namespace ExplorerHub.UI
{
    public class ChromeTabControl : ListBox
    {
        static ChromeTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabControl), new FrameworkPropertyMetadata(typeof(ChromeTabControl)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ChromeTabItem();
        }
    }
}

using System;
using System.Collections.Specialized;
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
            return new ChromeTabItem(this);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                var prevItem = (ChromeTabItem)ItemContainerGenerator.ContainerFromIndex(e.OldStartingIndex);
                prevItem.InvalidateVisual();
                var curItem = (ChromeTabItem)ItemContainerGenerator.ContainerFromIndex(e.NewStartingIndex);
                curItem.InvalidateVisual();
            }
        }
    }
}

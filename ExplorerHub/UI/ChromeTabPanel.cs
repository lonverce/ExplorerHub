using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ExplorerHub.UI
{
    public class ChromeTabPanel : Panel
    {
        static ChromeTabPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabPanel), new FrameworkPropertyMetadata(typeof(ChromeTabPanel)));
        }

        /// <summary>
        ///   在派生类中重写时，为 <see cref="T:System.Windows.FrameworkElement" /> 派生类定位子元素并确定大小。
        /// </summary>
        /// <param name="finalSize">父级中应使用此元素排列自身及其子元素的最终区域。</param>
        /// <returns>使用的实际大小。</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var finalRect = new Rect(finalSize);

            foreach (UIElement child in Children)
            {
                var childSize = child.DesiredSize;
                finalRect.Width = childSize.Width;
                finalRect.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                child.Arrange(finalRect);
                finalRect.X += childSize.Width;
            }

            return base.ArrangeOverride(finalSize);
        }

        [Bindable(false)]
        public double MaxTabWidth { get; set; }

        /// <summary>
        ///   测量每个Tab标签在布局中所需的大小，并确定自身的大小。
        /// </summary>
        /// <param name="availableSize">
        ///   此元素可提供给子元素的可用大小。
        ///    可指定无穷大作为一个值，该值指示元素将调整到适应内容的大小。
        /// </param>
        /// <returns>此元素基于其对子元素大小的计算确定它在布局期间所需要的大小。</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var children = Children;
            var childAvailableSizeWidth = double.IsPositiveInfinity(availableSize.Width) ? MaxTabWidth : Math.Max(MaxTabWidth, availableSize.Width / children.Count);
            var childAvailableSize = new Size(childAvailableSizeWidth, availableSize.Height);
            var measureSize = new Size();

            foreach (UIElement child in children)
            {
                child.Measure(childAvailableSize);
                var childDesiredSize = child.DesiredSize;
                measureSize.Width += childDesiredSize.Width;
                measureSize.Height = Math.Max(childDesiredSize.Height, measureSize.Height);
            }

            return measureSize;
        }
    }
}

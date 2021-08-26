using System;
using System.ComponentModel;
using System.Linq;
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

        [Bindable(false)]
        public Thickness Padding { get; set; } = new Thickness(0, 0, 0, 0);

        [Bindable(false)]
        public double MaxTabWidth { get; set; }

        [Bindable(false)]
        public double MinTabWidth { get; set; }

        /// <summary>
        ///   在派生类中重写时，为 <see cref="T:System.Windows.FrameworkElement" /> 派生类定位子元素并确定大小。
        /// </summary>
        /// <param name="finalSize">父级中应使用此元素排列自身及其子元素的最终区域。</param>
        /// <returns>使用的实际大小。</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var padding = Padding;
            var finalRect = new Rect(finalSize);
            var measureSize = new Size(padding.Left + padding.Right, padding.Top + padding.Bottom);
            finalRect.X += padding.Left;
            finalRect.Y += padding.Top;
            var children = Children;

            var childAvailableSizeWidth = double.IsPositiveInfinity(finalSize.Width) ? 
                MaxTabWidth : Math.Min(MaxTabWidth, (finalSize.Width-measureSize.Width) / children.Count);
            var childAvailableSizeHeight = double.IsPositiveInfinity(finalSize.Height)
                ? finalSize.Height
                : finalSize.Height - measureSize.Height;

            var hiddenCnt = 0;

            if (childAvailableSizeWidth < MinTabWidth)
            {
                var displayCnt = (int)Math.Floor((finalSize.Width - measureSize.Width) / MinTabWidth);
                hiddenCnt = children.Count - displayCnt;
                childAvailableSizeWidth = MinTabWidth;
            }

            // 前面几个不显示
            foreach (var child in Children.Cast<UIElement>().Take(hiddenCnt))
            {
                var childSize = child.DesiredSize;
                finalRect.Width = 0;
                finalRect.Height = Math.Max(childAvailableSizeHeight, childSize.Height);
                child.Arrange(finalRect);
            }

            foreach (var child in Children.Cast<UIElement>().Skip(hiddenCnt))
            {
                var childSize = child.DesiredSize;
                finalRect.Width = Math.Max(childAvailableSizeWidth, childSize.Width);
                finalRect.Height = Math.Max(childAvailableSizeHeight, childSize.Height);
                child.Arrange(finalRect);
                finalRect.X += finalRect.Width;
            }

            finalRect.Width += padding.Right;
            finalRect.Height += padding.Bottom;
            return finalSize;
        }

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
            var padding = Padding;
            var measureSize = new Size(padding.Left + padding.Right, padding.Top + padding.Bottom);
            var children = Children;
            var childAvailableSizeWidth = double.IsPositiveInfinity(availableSize.Width) ? 
                MaxTabWidth : Math.Min(MaxTabWidth, (availableSize.Width-measureSize.Width) / children.Count);
            var childAvailableSizeHeight = double.IsPositiveInfinity(availableSize.Height)
                ? availableSize.Height
                : availableSize.Height - measureSize.Height;

            var hiddenCnt = 0;

            if (childAvailableSizeWidth < MinTabWidth)
            {
                var displayCount = (int)Math.Floor((availableSize.Width - measureSize.Width) / MinTabWidth);
                hiddenCnt = children.Count - displayCount;
                childAvailableSizeWidth = MinTabWidth;
            }

            var childAvailableSize = new Size(childAvailableSizeWidth, childAvailableSizeHeight);
            
            // 前面几个不显示
            foreach (var child in children.Cast<UIElement>().Take(hiddenCnt))
            {
                child.Measure(Size.Empty);
            }

            foreach (var child in children.Cast<UIElement>().Skip(hiddenCnt))
            {
                child.Measure(childAvailableSize);
                var childDesiredSize = child.DesiredSize;
                measureSize.Width += Math.Max(childAvailableSizeWidth, childDesiredSize.Width);
                measureSize.Height = Math.Max(childDesiredSize.Height, measureSize.Height);
            }

            return measureSize;
        }
    }
}

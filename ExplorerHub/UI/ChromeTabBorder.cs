using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExplorerHub.UI
{
    public class ChromeTabBorder : Border
    {
        static ChromeTabBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabBorder), 
                new FrameworkPropertyMetadata(typeof(ChromeTabBorder)));
        }

        public static readonly DependencyProperty SplitBorderBrushProperty = DependencyProperty.Register(
            nameof(SplitBorderBrush), typeof(Brush), typeof(ChromeTabBorder), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, OnClearPenCache));

        private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public Brush SplitBorderBrush
        {
            get => (Brush)GetValue(SplitBorderBrushProperty);
            set => SetValue(SplitBorderBrushProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var border = this;
            var elementRect = new Rect(border.RenderSize);
            var pen = new Pen(border.Background, border.BorderThickness.Left);
            var parent = (ListBoxItem) TemplatedParent;
            if (parent.IsSelected || parent.IsMouseOver)
            {
                var geo = new StreamGeometry();
                using var gtx = geo.Open();
                gtx.BeginFigure(elementRect.BottomLeft, true, true);
                gtx.LineTo(elementRect.BottomLeft.TranslationX(-CornerRadius.TopLeft), true, true);
                gtx.ArcTo(elementRect.BottomLeft.TranslationY(-CornerRadius.TopLeft),
                    new Size(CornerRadius.TopLeft, CornerRadius.TopLeft), 0, false,
                    SweepDirection.Counterclockwise, true, true);

                gtx.BeginFigure(elementRect.BottomRight, true, true);
                gtx.LineTo(elementRect.BottomRight.TranslationX(CornerRadius.TopRight), true, true);
                gtx.ArcTo(elementRect.BottomRight.TranslationY(-CornerRadius.TopRight),
                    new Size(CornerRadius.TopRight, CornerRadius.TopRight), 0, false,
                    SweepDirection.Clockwise, true, true);

                dc.DrawGeometry(border.Background, pen, geo);
            }
            else if (SplitBorderBrush != null)
            {
                double thickness = 1;
                dc.DrawLine(new Pen(SplitBorderBrush, thickness), 
                    elementRect.TopRight.Translation(thickness/2, CornerRadius.TopRight), 
                    elementRect.BottomRight.Translation(thickness/2, -CornerRadius.TopRight));
            }
        }
    }
}

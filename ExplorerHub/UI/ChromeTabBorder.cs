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
            //var border = this;
            //var elementRect = new Rect(border.RenderSize);
            //var pen = new Pen(border.Background, border.BorderThickness.Left);
            //var parent = (ListBoxItem) TemplatedParent;
            //if (parent.IsSelected || parent.IsMouseOver)
            //{
            //    var geo = new StreamGeometry();
            //    using var gtx = geo.Open();
            //    var bottomLeft = elementRect.BottomLeft.Translation(0.5, 0.5);
            //    gtx.BeginFigure(bottomLeft, true, true);
            //    gtx.LineTo(bottomLeft.TranslationX(-CornerRadius.TopLeft), true, true);
            //    gtx.ArcTo(bottomLeft.TranslationY(-CornerRadius.TopLeft),
            //        new Size(CornerRadius.TopLeft, CornerRadius.TopLeft), 0, false,
            //        SweepDirection.Counterclockwise, true, true);

            //    var bottomRight = elementRect.BottomRight.Translation(-0.5, 0.5);
            //    gtx.BeginFigure(bottomRight, true, true);
            //    gtx.LineTo(bottomRight.TranslationX(CornerRadius.TopRight), true, true);
            //    gtx.ArcTo(bottomRight.TranslationY(-CornerRadius.TopRight),
            //        new Size(CornerRadius.TopRight, CornerRadius.TopRight), 0, false,
            //        SweepDirection.Clockwise, true, true);

            //    dc.DrawGeometry(border.Background, pen, geo);
            //}
            //else if (SplitBorderBrush != null)
            //{
            //    double thickness = 1;
            //    dc.DrawLine(new Pen(SplitBorderBrush, thickness), 
            //        elementRect.TopRight.Translation(thickness, CornerRadius.TopRight), 
            //        elementRect.BottomRight.Translation(thickness, -CornerRadius.TopRight));
            //}
        }
    }
}

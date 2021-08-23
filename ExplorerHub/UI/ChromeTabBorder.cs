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

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var border = this;
            
            var elementRect = new Rect(border.RenderSize);
            var pen = new Pen(border.Background, border.BorderThickness.Left);

            var geo = new StreamGeometry();
            using var gtx = geo.Open();
            var r = 5;
            var rediusSize = new Size(r, r);

            gtx.BeginFigure(elementRect.BottomLeft, true, true);
            gtx.LineTo(elementRect.BottomLeft.TranslationX(-r), true, true);
            gtx.ArcTo(elementRect.BottomLeft.TranslationY(-r),
                rediusSize, 0, false,
                SweepDirection.Counterclockwise, true, true);

            gtx.BeginFigure(elementRect.BottomRight, true, true);
            gtx.LineTo(elementRect.BottomRight.TranslationX(r), true, true);
            gtx.ArcTo(elementRect.BottomRight.TranslationY(-r), rediusSize, 0, false,
                SweepDirection.Clockwise, true, true);

            dc.DrawGeometry(border.Background, pen, geo);
        }
    }
}

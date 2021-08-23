using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ExplorerHub.Annotations;

namespace ExplorerHub.UI
{
    public class CornerAdorner : Adorner
    {
        public CornerAdorner([NotNull] UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var border = (Border) AdornedElement;
            var elementRect = new Rect(border.DesiredSize);
            var pen = new Pen(border.Background, 1);

            var geo = new StreamGeometry();
            using var gtx = geo.Open();
            var r = 10;
            gtx.BeginFigure(new Point(elementRect.BottomLeft.X, elementRect.BottomLeft.Y+r), true, true);
            gtx.LineTo(new Point(elementRect.BottomLeft.X-r, elementRect.BottomLeft.Y+r), true, true);
            gtx.ArcTo(elementRect.BottomLeft, new Size(r,r), 0, false, SweepDirection.Counterclockwise, true, true);
            drawingContext.DrawGeometry(border.Background, pen, geo);
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExplorerHub.UI
{
    public class ChromeTabItem : ListBoxItem
    {
        private readonly ChromeTabControl _tabControl;

        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            nameof(BorderCornerRadius), typeof(CornerRadius), 
            typeof(ChromeTabItem), new FrameworkPropertyMetadata(new CornerRadius(),FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DrawShapeProperty = DependencyProperty.Register(
            nameof(DrawShape), typeof(bool), typeof(ChromeTabItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DrawLineProperty = DependencyProperty.Register(
            nameof(DrawLine), typeof(bool), typeof(ChromeTabItem),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool DrawShape
        {
            get => (bool)GetValue(DrawShapeProperty);
            set => SetValue(DrawShapeProperty, value);
        }

        public bool DrawLine {
            get => (bool)GetValue(DrawLineProperty);
            set => SetValue(DrawLineProperty, value);
        }

        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }
        
        static ChromeTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabItem), new FrameworkPropertyMetadata(typeof(ChromeTabItem)));
        }

        public ChromeTabItem(ChromeTabControl tabControl)
        {
            _tabControl = tabControl;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var elementRect = new Rect(RenderSize);
            var pen = new Pen(Background, BorderThickness.Left);
            
            if (DrawShape)
            {
                var geo = new StreamGeometry();
                using var gtx = geo.Open();
                var bottomLeft = elementRect.BottomLeft;
                gtx.BeginFigure(bottomLeft, true, true);
                gtx.LineTo(bottomLeft.TranslationX(-BorderCornerRadius.TopLeft), true, true);
                gtx.ArcTo(bottomLeft.TranslationY(-BorderCornerRadius.TopLeft),
                    new Size(BorderCornerRadius.TopLeft, BorderCornerRadius.TopLeft), 0, false,
                    SweepDirection.Counterclockwise, true, true);

                var bottomRight = elementRect.BottomRight;
                gtx.BeginFigure(bottomRight, true, true);
                gtx.LineTo(bottomRight.TranslationX(BorderCornerRadius.TopRight), true, true);
                gtx.ArcTo(bottomRight.TranslationY(-BorderCornerRadius.TopRight),
                    new Size(BorderCornerRadius.TopRight, BorderCornerRadius.TopRight), 0, false,
                    SweepDirection.Clockwise, true, true);

                dc.DrawGeometry(Background, pen, geo);

                var idx = _tabControl.ItemContainerGenerator.IndexFromContainer(this);
                if (idx > 0)
                {
                    var item = (ChromeTabItem)_tabControl.ItemContainerGenerator.ContainerFromIndex(idx - 1);
                    if (!item.DrawShape && item.DrawLine)
                    {
                        item.DrawLine = false;
                    }
                }

                DrawLine = true;
            }
            else 
            {
                var idx = _tabControl.ItemContainerGenerator.IndexFromContainer(this);

                // pre-check
                if (DrawLine)
                {
                    if (idx + 1 < _tabControl.Items.Count)
                    {
                        var nextItem = (ChromeTabItem)_tabControl.ItemContainerGenerator.ContainerFromIndex(idx + 1);
                        if (nextItem.DrawShape)
                        {
                            DrawLine = false;
                        }
                    }
                }
                else
                {
                    if (idx + 1 < _tabControl.Items.Count)
                    {
                        var nextItem = (ChromeTabItem)_tabControl.ItemContainerGenerator.ContainerFromIndex(idx + 1);
                        if (!nextItem.DrawShape)
                        {
                            DrawLine = true;
                        }
                    }
                    else
                    {
                        DrawLine = true;
                    }
                }

                if (DrawLine)
                {
                    double thickness = 1;
                    dc.DrawLine(new Pen(BorderBrush, thickness),
                        elementRect.TopRight.Translation(0, BorderCornerRadius.TopRight),
                        elementRect.BottomRight.Translation(0, -BorderCornerRadius.TopRight));
                }

                if (idx > 0)
                {
                    var item = (ChromeTabItem)_tabControl.ItemContainerGenerator.ContainerFromIndex(idx - 1);
                    if (!item.DrawShape)
                    {
                        item.DrawLine = true;
                    }
                }
            }
        }
    }
}

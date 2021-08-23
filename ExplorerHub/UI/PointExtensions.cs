using System.Windows;

namespace ExplorerHub.UI
{
    public static class PointExtensions
    {
        public static Point TranslationX(this Point p, double offset)
        {
            return new Point(p.X + offset, p.Y);
        }

        public static Point TranslationY(this Point p, double offset)
        {
            return new Point(p.X, p.Y + offset);
        }
    }
}
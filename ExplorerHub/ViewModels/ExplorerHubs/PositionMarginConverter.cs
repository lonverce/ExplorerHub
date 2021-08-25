using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ExplorerHub.ViewModels.Explorers;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class PositionMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ItemPositionType position) || targetType != typeof(Thickness) || !(parameter is Thickness margin))
            {
                return new Thickness(0, 0, 0, 0);
            }

            var marginLeft = position.HasFlag(ItemPositionType.Head) ? margin.Left : 0;
            var marginRight = position.HasFlag(ItemPositionType.Tail) ? margin.Right : 0;
            return new Thickness(marginLeft, margin.Top, marginRight, margin.Bottom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WindowStateMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WindowState wndState && targetType == typeof(Thickness))
            {
                var marginTop = wndState == WindowState.Maximized ? 1 : System.Convert.ToInt32(parameter);
                return new Thickness(0, marginTop, 0, 0);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

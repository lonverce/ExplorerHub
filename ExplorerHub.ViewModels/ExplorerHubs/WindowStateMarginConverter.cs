using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
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
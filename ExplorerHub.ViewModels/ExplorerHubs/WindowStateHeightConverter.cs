using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ExplorerHub.ViewModels.ExplorerHubs
{
    public class WindowStateHeightConverter : IValueConverter
    {
        public double NormalHeight { get; set; }
        public double MaxHeight { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WindowState wndState && targetType == typeof(double))
            {
                return wndState == WindowState.Maximized ? MaxHeight : NormalHeight;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace ExplorerHub.UI
{
    public class MaxWidthConverter : IValueConverter
    {
        public double PreservedSize { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var wndWidth = System.Convert.ToDouble(value);
            return wndWidth - PreservedSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

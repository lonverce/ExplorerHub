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
            if (value is ItemPositionType position && targetType == typeof(Thickness))
            {
                var distance = System.Convert.ToInt32(parameter);
                var marginRight = position.HasFlag(ItemPositionType.Tail) ? distance : -distance;
                return new Thickness(distance, 0, marginRight, 0);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

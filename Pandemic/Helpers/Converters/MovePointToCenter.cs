﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Pandemic.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class MovePointToCenter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + (double)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)parameter;
        }
    }
}
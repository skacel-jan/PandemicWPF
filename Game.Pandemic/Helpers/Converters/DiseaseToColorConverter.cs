using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Game.Pandemic.GameLogic;

namespace Game.Pandemic.Helpers.Converters
{
    [ValueConversion(typeof(DiseaseColor), typeof(SolidColorBrush))]
    public class DiseaseToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((DiseaseColor)value) switch
            {
                DiseaseColor.Yellow => new SolidColorBrush(Colors.Yellow),
                DiseaseColor.Red => new SolidColorBrush(Colors.Red),
                DiseaseColor.Blue => new SolidColorBrush(Colors.Blue),
                DiseaseColor.Black => new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30)),
                _ => new SolidColorBrush(Colors.Yellow),
            };
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return DiseaseColor.Yellow;
        }
    }
}
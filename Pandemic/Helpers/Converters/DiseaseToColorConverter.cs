using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Pandemic.Converters
{
    [ValueConversion(typeof(DiseaseColor), typeof(SolidColorBrush))]
    public class DiseaseToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            switch ((DiseaseColor)value)
            {
                case DiseaseColor.Yellow:
                    return new SolidColorBrush(Colors.Yellow);
                case DiseaseColor.Red:
                    return new SolidColorBrush(Colors.Red);
                case DiseaseColor.Blue:
                    return new SolidColorBrush(Colors.Blue);
                case DiseaseColor.Black:
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(30,30,30));
                default:
                    return new SolidColorBrush(Colors.Yellow);
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return DiseaseColor.Yellow;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static PandemicLegacy.Common;

namespace PandemicLegacy.Views
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
                    return new SolidColorBrush(Colors.DarkGray);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PandemicLegacy.UserControls
{
    /// <summary>
    /// Interaction logic for InfectionRateTracker.xaml
    /// </summary>
    public partial class InfectionRateTracker : UserControl
    {
        /// <summary>
        /// Gets or sets the Label which is displayed next to the field
        /// </summary>
        public int InfectionPosition
        {
            get { return (int)GetValue(InfectionPositionProperty); }
            set { SetValue(InfectionPositionProperty, value); }
        }

        /// <summary>
        /// Identified the InfectionPosition dependency property
        /// </summary>
        public static readonly DependencyProperty InfectionPositionProperty =
            DependencyProperty.Register("InfectionPosition", typeof(int),
              typeof(InfectionRateTracker), new PropertyMetadata(0, null, new CoerceValueCallback(OnInfectionPositionCoerceValue)));


        private static object OnInfectionPositionCoerceValue(DependencyObject d, object value)
        {
            int position = (int)value;

            return position < 0 ? 0 : position > 6 ? 6 : position;
        }

        public InfectionRateTracker()
        {
            InitializeComponent();
        }
    }
}

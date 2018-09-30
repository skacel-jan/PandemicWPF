using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Pandemic.Views
{
    /// <summary>
    /// Interaction logic for WorldMap.xaml
    /// </summary>
    public partial class WorldMap : UserControl
    {
        public WorldMap()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CitiesProperty =
            DependencyProperty.Register(nameof(Cities), typeof(IDictionary<string, MapCity>), typeof(WorldMap), new PropertyMetadata(null));

        public IDictionary<string, MapCity> Cities
        {
            get { return (IDictionary<string, MapCity>)GetValue(CitiesProperty); }
            set { SetValue(CitiesProperty, value); }
        }
    }
}
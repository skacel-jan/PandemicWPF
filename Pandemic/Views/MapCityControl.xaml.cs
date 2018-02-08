using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Threading;

namespace Pandemic.Views
{
    /// <summary>
    /// Interaction logic for MapCityControl.xaml
    /// </summary>
    public partial class MapCityControl : UserControl
    {
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(MapCityControl), new UIPropertyMetadata(null));


        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(MapCityControl), new UIPropertyMetadata(null));

        public string CityName
        {
            get { return (string)GetValue(CityNameProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public static readonly DependencyProperty CityNameProperty =
            DependencyProperty.Register(nameof(CityName), typeof(string), typeof(MapCityControl), new PropertyMetadata(string.Empty));

        public DiseaseColor CityColor
        {
            get { return (DiseaseColor)GetValue(CityColorProperty); }
            set { SetValue(CityColorProperty, value); }
        }

        public static readonly DependencyProperty CityColorProperty =
            DependencyProperty.Register(nameof(CityColor), typeof(DiseaseColor), typeof(MapCityControl), new PropertyMetadata(DiseaseColor.Yellow));

        public bool HasResearchStation
        {
            get { return (bool)GetValue(HasResearchStationProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public static readonly DependencyProperty HasResearchStationProperty =
            DependencyProperty.Register(nameof(HasResearchStation), typeof(bool), typeof(MapCityControl), new PropertyMetadata(false));

        public int YellowInfection
        {
            get { return (int)GetValue(YellowInfectionProperty); }
            set => SetValue(YellowInfectionProperty, value);
        }

        public static readonly DependencyProperty YellowInfectionProperty =
            DependencyProperty.Register(nameof(YellowInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public int RedInfection
        {
            get { return (int)GetValue(RedInfectionProperty); }
            set { SetValue(RedInfectionProperty, value); }
        }

        public static readonly DependencyProperty RedInfectionProperty =
            DependencyProperty.Register(nameof(RedInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public int BlueInfection
        {
            get { return (int)GetValue(BlueInfectionProperty); }
            set { SetValue(BlueInfectionProperty, value); }
        }

        public static readonly DependencyProperty BlueInfectionProperty =
            DependencyProperty.Register(nameof(BlueInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public int BlackInfection
        {
            get { return (int)GetValue(BlackInfectionProperty); }
            set { SetValue(BlackInfectionProperty, value); }
        }

        public static readonly DependencyProperty BlackInfectionProperty =
            DependencyProperty.Register(nameof(BlackInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public ObservableCollection<Character> Characters
        {
            get { return GetValue(CharactersProperty) as ObservableCollection<Character>; }
            set { SetValue(CharactersProperty, value); }
        }

        public static readonly DependencyProperty CharactersProperty =
            DependencyProperty.Register(nameof(Characters), typeof(ObservableCollection<Character>), typeof(MapCityControl), new PropertyMetadata(null));

        public MapCityControl()
        {
            InitializeComponent();
        }
    }
}

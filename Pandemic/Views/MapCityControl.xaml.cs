using Pandemic.GameLogic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pandemic.Views
{
    /// <summary>
    /// Interaction logic for MapCityControl.xaml
    /// </summary>
    public partial class MapCityControl : UserControl
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand),
            typeof(ICommand), typeof(MapCityControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(MapCityControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty CityNameProperty =
            DependencyProperty.Register(nameof(CityName), typeof(string), typeof(MapCityControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CityColorProperty =
            DependencyProperty.Register(nameof(CityColor), typeof(DiseaseColor), typeof(MapCityControl), new PropertyMetadata(DiseaseColor.Yellow));

        public static readonly DependencyProperty HasResearchStationProperty =
            DependencyProperty.Register(nameof(HasResearchStation), typeof(bool), typeof(MapCityControl), new PropertyMetadata(false));

        public static readonly DependencyProperty YellowInfectionProperty =
            DependencyProperty.Register(nameof(YellowInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public static readonly DependencyProperty RedInfectionProperty =
            DependencyProperty.Register(nameof(RedInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public static readonly DependencyProperty BlueInfectionProperty =
            DependencyProperty.Register(nameof(BlueInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public static readonly DependencyProperty BlackInfectionProperty =
            DependencyProperty.Register(nameof(BlackInfection), typeof(int), typeof(MapCityControl), new PropertyMetadata(0));

        public static readonly DependencyProperty CharactersProperty =
            DependencyProperty.Register(nameof(Characters), typeof(ObservableCollection<Character>), typeof(MapCityControl),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsBorderVisibleProperty =
            DependencyProperty.Register(nameof(IsBorderVisible), typeof(bool), typeof(MapCityControl), new PropertyMetadata(null));

        public static readonly DependencyProperty AnchorXProperty =
            DependencyProperty.Register(nameof(AnchorX), typeof(double), typeof(MapCityControl), new FrameworkPropertyMetadata(AnchorCoordinateChanged));

        private static void AnchorCoordinateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapCity = (MapCityControl)d;
            mapCity.CenterPoint = new Point(mapCity.AnchorX + mapCity.ActualWidth / 2, mapCity.AnchorY + mapCity.ActualHeight / 2);
        }

        public static readonly DependencyProperty AnchorYProperty =
            DependencyProperty.Register(nameof(AnchorY), typeof(double), typeof(MapCityControl), new FrameworkPropertyMetadata(AnchorCoordinateChanged));

        public static DependencyProperty CenterPointProperty = DependencyProperty.Register(nameof(CenterPoint), typeof(Point), typeof(MapCityControl),
            new FrameworkPropertyMetadata(default));

        public MapCityControl()
        {
            InitializeComponent();
        }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        public string CityName
        {
            get { return (string)GetValue(CityNameProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public DiseaseColor CityColor
        {
            get { return (DiseaseColor)GetValue(CityColorProperty); }
            set { SetValue(CityColorProperty, value); }
        }

        public bool HasResearchStation
        {
            get { return (bool)GetValue(HasResearchStationProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public int YellowInfection
        {
            get { return (int)GetValue(YellowInfectionProperty); }
            set => SetValue(YellowInfectionProperty, value);
        }

        public int RedInfection
        {
            get { return (int)GetValue(RedInfectionProperty); }
            set { SetValue(RedInfectionProperty, value); }
        }

        public int BlueInfection
        {
            get { return (int)GetValue(BlueInfectionProperty); }
            set { SetValue(BlueInfectionProperty, value); }
        }

        public int BlackInfection
        {
            get { return (int)GetValue(BlackInfectionProperty); }
            set { SetValue(BlackInfectionProperty, value); }
        }

        public ObservableCollection<Character> Characters
        {
            get { return GetValue(CharactersProperty) as ObservableCollection<Character>; }
            set { SetValue(CharactersProperty, value); }
        }

        public bool IsBorderVisible
        {
            get { return (bool)GetValue(IsBorderVisibleProperty); }
            set { SetValue(IsBorderVisibleProperty, value); }
        }

        public double AnchorX
        {
            get { return (double)GetValue(AnchorXProperty); }
            set { SetValue(AnchorXProperty, value); }
        }

        public double AnchorY
        {
            get { return (double)GetValue(AnchorYProperty); }
            set { SetValue(AnchorYProperty, value); }
        }

        public Point CenterPoint
        {
            get { return (Point)GetValue(CenterPointProperty); }
            set { SetValue(CenterPointProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.SizeChanged += MapCityControl_SizeChanged;
        }

        private void MapCityControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CenterPoint = new Point(AnchorX + ActualWidth / 2, AnchorY + ActualHeight / 2);
        }
    }
}
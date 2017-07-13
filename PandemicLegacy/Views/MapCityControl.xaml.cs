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

namespace PandemicLegacy.Views
{
    /// <summary>
    /// Interaction logic for MapCityControl.xaml
    /// </summary>
    public partial class MapCityControl : UserControl
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MapCityControl), new UIPropertyMetadata(null));

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

        public ObservableCollection<Pawn> Pawns
        {
            get { return GetValue(PawnsProperty) as ObservableCollection<Pawn>; }
            set { SetValue(PawnsProperty, value); }
        }

        public static readonly DependencyProperty PawnsProperty =
            DependencyProperty.Register(nameof(Pawns), typeof(ObservableCollection<Pawn>), typeof(MapCityControl), new PropertyMetadata(null, PawnsChanged));

        private List<Ellipse> ellipses;

        private static void PawnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapCityControl = d as MapCityControl;

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                // Unsubscribe from CollectionChanged on the old collection
                coll.CollectionChanged -= mapCityControl.Pawns_CollectionChanged;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<Pawn>)e.NewValue;
                // Subscribe to CollectionChanged on the new collection
                coll.CollectionChanged += mapCityControl.Pawns_CollectionChanged;
            }

            if (mapCityControl.Pawns?.Count > 0)
                mapCityControl.LayoutItemsInGrid();
        }

        private void Pawns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var ellipse in ellipses)
            {
                MainGrid.Children.Remove(ellipse);
            }
            ellipses.Clear();

            if (Pawns.Count > 0)
                LayoutItemsInGrid();
        }

        private void LayoutItemsInGrid()
        {
            int column = 0;
            foreach (var pawn in Pawns)
            {
                Ellipse ellipse = new Ellipse()
                {
                    Fill = new SolidColorBrush() { Color = pawn.Color, Opacity = 0.8 },
                    IsHitTestVisible = false
                };

                Grid.SetColumn(ellipse, column++);
                Grid.SetRow(ellipse, 1);
                Grid.SetRowSpan(ellipse, 3);

                ellipses.Add(ellipse);

                MainGrid.Children.Add(ellipse);
            }
        }

        public MapCityControl()
        {
            InitializeComponent();
            ellipses = new List<Ellipse>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Command?.Execute(null);
        }
    }
}

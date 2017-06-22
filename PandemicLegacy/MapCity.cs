using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PandemicLegacy
{
    public class MapCity : ViewModelBase
    {
        private City _city;
        public City City { get { return _city; } set { Set(ref _city, value); } }

        private bool _hasResearchStation;
        public bool HasResearchStation { get { return _hasResearchStation; } set { Set(ref _hasResearchStation, value); } }

        private IEnumerable<MapCity> ConnectedCities { get; set; }

        private int _yellowInfection;
        public int YellowInfection
        {
            get => _yellowInfection;
            set => Set(ref _yellowInfection, value);
        }

        private int _redInfection;
        public int RedInfection
        {
            get => _redInfection;
            set => Set(ref _redInfection, value);
        }

        private int _blueInfection;
        public int BlueInfection
        {
            get => _blueInfection;
            set => Set(ref _blueInfection, value);
        }

        private int _blackInfection;
        public int BlackInfection
        {
            get => _blackInfection;
            set => Set(ref _blackInfection, value);
        }

        private int _population;
        public int Population { get { return _population; } private set { Set(ref _population, value); } }
        public double Area { get; private set; }

        public ICommand CityCommand { get; set; }

        public ObservableCollection<Pawn> Pawns { get; }

        public MapCity(City city)
        {
            this.City = city;

            CityCommand = new RelayCommand(() => CityButtonClicked(), () => IsCityEnabled());
            Pawns = new ObservableCollection<Pawn>();
        }

        public bool IsCityConnected(MapCity toCity)
        {
            return ConnectedCities.Contains(toCity);
        }

        public void ChangeInfection(DiseaseColor color, int value)
        {
            if (value < 0) value = 0;
            else if (value > 3) value = 3;

            switch (color)
            {
                case DiseaseColor.Yellow:
                    YellowInfection = value;
                    break;
                case DiseaseColor.Red:
                    RedInfection = value;
                    break;
                case DiseaseColor.Blue:
                    BlueInfection = value;
                    break;
                case DiseaseColor.Black:
                    BlackInfection = value;
                    break;
            }
        }

        public void RemoveInfection(DiseaseColor color)
        {
            ChangeInfection(color, 0);
        }

        public void AddConnectedCities(params MapCity[] cities)
        {
            this.ConnectedCities = cities.ToList();
        }

        public override string ToString()
        {
            return City.ToString();
        }

        public bool IsEnabled => false;

        private bool IsCityEnabled()
        {
            return true;
        }

        private void CityButtonClicked()
        {
            MessengerInstance.Send(this, "CityClicked");
        }
    }
}

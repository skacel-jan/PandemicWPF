using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PandemicLegacy.Common;

namespace PandemicLegacy
{
    public class MapCity : ViewModelBase
    {
        private City _city;
        public City City { get { return _city; } set { Set(ref _city, value); } }

        private bool _hasResearchStation;
        public bool HasResearchStation { get { return _hasResearchStation; } set { Set(ref _hasResearchStation, value); } }

        private IEnumerable<MapCity> ConnectedCities { get; set; }
        private IDictionary<DiseaseColor, int> Infections { get; set; }

        private int _population;
        public int Population { get { return _population; } private set { Set(ref _population, value); } }
        public double Area { get; private set; }

        public ICommand CityCommand { get; set; }

        private Pawn _pawn;
        public Pawn Pawn
        {
            get { return _pawn; }
            set
            {
                Set(ref _pawn, value);
            }
        }

        public MapCity(City city)
        {
            this.City = city;

            this.Infections = new Dictionary<DiseaseColor, int>(4)
            {
                {DiseaseColor.Black, 0 },
                {DiseaseColor.Blue, 0 },
                {DiseaseColor.Red, 0 },
                {DiseaseColor.Yellow, 0 }
            };

            CityCommand = new RelayCommand(() => CityButtonClicked(), () => IsCityEnabled());
        }

        public bool IsCityConnected(MapCity toCity)
        {
            return ConnectedCities.Contains(toCity);
        }

        public void ChangeInfection(DiseaseColor color, int value)
        {
            if (value < 0)
                this.Infections[color] = 0;
            else if (value > 3)
                this.Infections[color] = 3;
            else
                this.Infections[color] = value;
        }

        public void RemoveInfection(DiseaseColor color)
        {
            this.Infections[color] = 0;
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
            HasResearchStation = true;
            MessengerInstance.Send(this, "CityClicked");
        }
    }
}

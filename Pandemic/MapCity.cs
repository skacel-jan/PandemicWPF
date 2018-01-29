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

namespace Pandemic
{
    public class MapCity : ViewModelBase
    {
        private City _city;
        public City City { get { return _city; } set { Set(ref _city, value); } }

        private bool _hasResearchStation;
        public bool HasResearchStation { get { return _hasResearchStation; } set { Set(ref _hasResearchStation, value); } }

        public IEnumerable<MapCity> ConnectedCities { get; set; }

        private int _yellowInfection;
        public int YellowInfection
        {
            get => _yellowInfection;
            set
            {
                Set(ref _yellowInfection, value);
                CoerceInfection(ref _yellowInfection);
            }
        }

        private void CoerceInfection(ref int infection)
        {
            if (infection < 0)
                infection = 0;
            else if (infection > 3)
                infection = 3;
        }

        private int _redInfection;
        public int RedInfection
        {
            get => _redInfection;
            set
            {
                Set(ref _redInfection, value);
                CoerceInfection(ref _redInfection);
            }
        }

        private int _blueInfection;
        public int BlueInfection
        {
            get => _blueInfection;
            set
            {
                Set(ref _blueInfection, value);
                CoerceInfection(ref _blueInfection);
            }
        }

        private int _blackInfection;
        public int BlackInfection
        {
            get => _blackInfection;
            set
            {
                Set(ref _blackInfection, value);
                CoerceInfection(ref _blackInfection);
            }
        }

        private int _population;
        public int Population { get { return _population; } private set { Set(ref _population, value); } }
        public double Area { get; private set; }

        public ICommand CityCommand { get; set; }

        public ObservableCollection<Pawn> Pawns { get; }

        public MapCity(City city)
        {
            this.City = city;

            CityCommand = new RelayCommand(() => CityButtonClicked(), () => IsEnabled);
            Pawns = new ObservableCollection<Pawn>();
        }

        public void PawnsChanged()
        {
            RaisePropertyChanged(nameof(Pawns));
        }

        public bool IsCityConnected(MapCity toCity)
        {
            return ConnectedCities.Contains(toCity);
        }

        public int ChangeInfection(DiseaseColor color, int value)
        {
            if (value < 0) value = 0;
            else if (value > 3) value = 3;

            int removedInfection = 0;
            switch (color)
            {
                case DiseaseColor.Yellow:
                    removedInfection = YellowInfection - value;
                    YellowInfection = value;
                    break;
                case DiseaseColor.Red:
                    removedInfection = RedInfection - value;
                    RedInfection = value;
                    break;
                case DiseaseColor.Blue:
                    removedInfection = BlueInfection - value;
                    BlueInfection = value;
                    break;
                case DiseaseColor.Black:
                    removedInfection = BlackInfection - value;
                    BlackInfection = value;
                    break;
            }
            return Math.Abs(removedInfection);
        }

        public bool RaiseInfection(DiseaseColor color)
        {
            bool isOutbreak = false;
            switch (color)
            {
                case DiseaseColor.Yellow:
                    isOutbreak = YellowInfection == 3;
                    YellowInfection += 1;
                    break;
                case DiseaseColor.Red:
                    isOutbreak = RedInfection == 3;
                    RedInfection += 1;
                    break;
                case DiseaseColor.Blue:
                    isOutbreak = BlueInfection == 3;
                    BlueInfection += 1;
                    break;
                case DiseaseColor.Black:
                    isOutbreak = BlackInfection == 3;
                    BlackInfection += 1;
                    break;
            }
            return isOutbreak;
        }

        public void DecreaseInfection(DiseaseColor color)
        {
            switch (color)
            {
                case DiseaseColor.Yellow:
                    YellowInfection--;
                    break;
                case DiseaseColor.Red:
                    RedInfection--;
                    break;
                case DiseaseColor.Blue:
                    BlueInfection--;
                    break;
                case DiseaseColor.Black:
                    BlackInfection--;
                    break;
            }
        }

        public int RemoveInfection(DiseaseColor color)
        {
            return ChangeInfection(color, 0);
        }

        public void AddConnectedCities(params MapCity[] cities)
        {
            this.ConnectedCities = cities.ToList();
        }

        public override string ToString()
        {
            return City.ToString();
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        private bool _canMove = true;
        public bool CanMove
        {
            get => _canMove;
            set => Set(ref _canMove, value);
        }

        private void CityButtonClicked()
        {
            MessengerInstance.Send(this, "CityClicked");
        }
    }
}

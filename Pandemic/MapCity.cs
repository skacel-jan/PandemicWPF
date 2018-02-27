using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Pandemic
{
    public class MapCity : ViewModelBase
    {
        private bool _canMove = true;
        private City _city;
        private bool _hasResearchStation;

        private IDictionary<DiseaseColor, int> _infections;
        private bool _isEnabled = true;

        private int _population;

        public MapCity(City city, IDictionary<DiseaseColor, Disease> diseases)
        {
            City = city ?? throw new ArgumentNullException("city");
            Diseases = diseases ?? throw new ArgumentNullException("diseases");

            CityCommand = new RelayCommand(() => MapCitySelected(), () => IsEnabled);
            InstantMoveCommand = new RelayCommand(() => InstantMove());
            Characters = new ObservableCollection<Character>();
            Infections = new Dictionary<DiseaseColor, int>(4)
            {
                {DiseaseColor.Black, 0 },
                {DiseaseColor.Blue, 0 },
                {DiseaseColor.Red, 0 },
                {DiseaseColor.Yellow, 0 }
            };
        }

        public double Area { get; private set; }

        public int BlackInfection
        {
            get => _infections[DiseaseColor.Black];
        }

        public int BlueInfection
        {
            get => _infections[DiseaseColor.Blue];
        }

        public bool CanMove
        {
            get => _canMove;
            set => Set(ref _canMove, value);
        }

        public City City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        public ICommand CityCommand { get; set; }

        public ICommand InstantMoveCommand { get; set; }

        public IEnumerable<MapCity> ConnectedCities { get; set; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; private set; }

        public bool HasResearchStation
        {
            get => _hasResearchStation;
            set => Set(ref _hasResearchStation, value);
        }

        public ObservableCollection<Character> Characters { get; }

        public IDictionary<DiseaseColor, int> Infections
        {
            get { return _infections; }
            set { _infections = value; }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        public int Population
        {
            get => _population;
            private set => Set(ref _population, value);
        }

        public int RedInfection
        {
            get => _infections[DiseaseColor.Red];
        }

        public int YellowInfection
        {
            get => _infections[DiseaseColor.Yellow];
        }

        public void AddConnectedCities(params MapCity[] cities)
        {
            ConnectedCities = cities.ToList();
        }

        public int ChangeInfection(DiseaseColor color, int value)
        {
            if (value < -3 || value > 3)
            {
                throw new ArgumentException("Value cannot be lesser then -3 or greater then 3", "value");
            }

            int oldInfections = Infections[color];
            var newInfection = CoerceInfection(oldInfections + value);
            if (_infections[color] != newInfection)
            {
                _infections[color] = newInfection;

                switch (color)
                {
                    case DiseaseColor.Black:
                        RaisePropertyChanged(nameof(BlackInfection));
                        break;

                    case DiseaseColor.Blue:
                        RaisePropertyChanged(nameof(BlueInfection));
                        break;

                    case DiseaseColor.Red:
                        RaisePropertyChanged(nameof(RedInfection));
                        break;

                    case DiseaseColor.Yellow:
                        RaisePropertyChanged(nameof(YellowInfection));
                        break;
                }
            }

            return Math.Abs(oldInfections - Infections[color]);
        }

        public void CharactersChanged()
        {
            RaisePropertyChanged(nameof(Characters));
        }

        public bool IsCityConnected(MapCity toCity)
        {
            return ConnectedCities.Contains(toCity);
        }

        public int RemoveInfection(DiseaseColor color)
        {
            return ChangeInfection(color, -3);
        }

        public override string ToString()
        {
            return City.ToString();
        }

        internal void RemoveCuredInfections()
        {
            foreach (var disease in Diseases.Values)
            {
                if (disease.IsCured)
                {
                    RemoveInfection(disease.Color);
                }
            }
        }

        internal int TreatDisease(DiseaseColor diseaseColor)
        {
            if (Diseases[diseaseColor].IsCured)
            {
                return RemoveInfection(diseaseColor);
            }
            else
            {
                return ChangeInfection(diseaseColor, -1);
            }
        }

        private int CoerceInfection(int infection)
        {
            if (infection < 0)
            {
                return 0;
            }
            else if (infection > 3)
            {
                return 3;
            }
            else
            {
                return infection;
            }
        }

        private void MapCitySelected()
        {
            MessengerInstance.Send(this, Messenger.CitySelected);
        }

        private void InstantMove()
        {
            MessengerInstance.Send(this, Messenger.InstantMove);
        }
    }
}
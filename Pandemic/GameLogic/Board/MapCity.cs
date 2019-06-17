using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Pandemic
{
    public class MapCity : ObservableObject
    {
        private City _city;
        private ICommand _cityCommand;
        private bool _hasResearchStation;
        private ICommand _instantMoveCommand;
        private bool _isSelectable = false;       

        public MapCity(City city, IDictionary<DiseaseColor, Disease> diseases)
        {
            City = city ?? throw new ArgumentNullException(nameof(city));
            Diseases = diseases ?? throw new ArgumentNullException(nameof(diseases));

            Characters = new ObservableCollection<Character>();
            Infections = new Dictionary<DiseaseColor, int>(4)
            {
                {DiseaseColor.Black, 0 },
                {DiseaseColor.Blue, 0 },
                {DiseaseColor.Red, 0 },
                {DiseaseColor.Yellow, 0 }
            };

            DiseasesToTreat = new List<DiseaseColor>();

            ConnectedCities = new List<MapCity>();
        }

        public event EventHandler CitySelected;

        public event EventHandler CityDoubleClicked;

        public double Area { get; }

        public int BlackInfection
        {
            get => Infections[DiseaseColor.Black];
        }

        public int BlueInfection
        {
            get => Infections[DiseaseColor.Blue];
        }

        public City City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        public ICommand CityCommand
        {
            get => _cityCommand ?? (_cityCommand = new RelayCommand(() => MapCitySelected()));
        }

        public IList<MapCity> ConnectedCities { get; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; }

        public IList<DiseaseColor> DiseasesToTreat { get; }

        public bool HasResearchStation
        {
            get => _hasResearchStation;
            set => Set(ref _hasResearchStation, value);
        }

        public ObservableCollection<Character> Characters { get; }

        public IDictionary<DiseaseColor, int> Infections { get; }

        public ICommand InstantMoveCommand
        {
            get => _instantMoveCommand ?? (_instantMoveCommand = new RelayCommand(() => InstantMove()));
        }

        public bool IsSelectable
        {
            get => _isSelectable;
            set => Set(ref _isSelectable, value);
        }


        private Point _centerPoint;

        /// <summary>
        /// Sets and gets the Point property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Point CenterPoint
        {
            get => _centerPoint;
            set => Set(ref _centerPoint, value);
        }

        public int Population { get; }

        public int RedInfection
        {
            get => Infections[DiseaseColor.Red];
        }

        public int YellowInfection
        {
            get => Infections[DiseaseColor.Yellow];
        }

        public void AddConnectedCity(MapCity city)
        {
            ConnectedCities.Add(city);
        }

        public int ChangeInfection(DiseaseColor color, int value)
        {
            if (value < -3 || value > 3)
            {
                throw new ArgumentException("Value cannot be lesser then -3 or greater then 3", nameof(value));
            }

            int oldInfections = Infections[color];
            var newInfection = CoerceInfection(oldInfections + value);
            if (Infections[color] != newInfection)
            {
                Infections[color] = newInfection;

                ChangeDiseasesToTreat(color);

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

        internal int TreatDisease(DiseaseColor diseaseColor)
        {
            if (Diseases[diseaseColor].Status > Disease.State.NotCured)
            {
                return RemoveInfection(diseaseColor);
            }
            else
            {
                return ChangeInfection(diseaseColor, -1);
            }
        }

        protected virtual void OnCitySelected(EventArgs e)
        {
            CitySelected?.Invoke(this, e);
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

        private void ChangeDiseasesToTreat(DiseaseColor color)
        {
            if (Infections[color] > 0 && !DiseasesToTreat.Contains(color))
            {
                DiseasesToTreat.Add(color);
            }
            else if (Infections[color] == 0 && DiseasesToTreat.Contains(color))
            {
                DiseasesToTreat.Remove(color);
            }
        }

        private void InstantMove()
        {
            OnMovedToCity(EventArgs.Empty);
        }

        private protected virtual void OnMovedToCity(EventArgs e)
        {
            CityDoubleClicked?.Invoke(this, e);
        }

        private void MapCitySelected()
        {
            OnCitySelected(EventArgs.Empty);
        }
    }
}
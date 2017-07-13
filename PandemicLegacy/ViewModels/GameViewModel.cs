using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PandemicLegacy.Characters;
using PandemicLegacy.Decks;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace PandemicLegacy.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ICommand MoveActionCommand { get; set; }
        public ICommand TreatActionCommand { get; set; }
        public ICommand ShareActionCommand { get; set; }
        public ICommand BuildActionCommand { get; set; }
        public ICommand DiscoverCureActionCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CardCommand { get; set; }

        public Board Board { get; private set; }

        public int BoardInfectionPosition => Board.InfectionPosition;

        private Character _currentCharacter;
        public Character CurrentCharacter
        {
            get { return _currentCharacter; }
            set { Set(ref _currentCharacter, value); }
        }

        private bool _isInfoVisible;
        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { Set(ref _isInfoVisible, value); }
        }

        private string _infoText;
        public string InfoText
        {
            get { return _infoText; }
            set
            {
                Set(ref _infoText, value);
                IsInfoVisible = string.IsNullOrEmpty(value) ? false : true;
            }
        }

        private bool _moveSelected;
        private MapCity _destinationCity;

        public GameViewModel()
        {
            var mapFactory = new WorldMapFactory();
            var cities = mapFactory.GetCities();

            Board = new Board(mapFactory.BuildMap(), new InfectionDeck(cities), new PlayerDeck(cities));
            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            DiscoverCureActionCommand = new RelayCommand(DiscoverCure, CanDiscoverCure);
            BuildActionCommand = new RelayCommand(BuildStructure, CanBuildStructure);
            MoveActionCommand = new RelayCommand(MoveActionSelected);
            CancelCommand = new RelayCommand(Cancel);
            CardCommand = new RelayCommand<PlayerCard>(CardSelected);

            CurrentCharacter = new Medic()
            {
                Player = new Player() { Pawn = new Pawn(Colors.Brown) },
                MapCity = Board.WorldMap.Cities[City.Atlanta]
            };

            Board.WorldMap.Cities[City.Atlanta].HasResearchStation = true;

            PlayerCard card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            MessengerInstance.Register<MapCity>(this, "CityClicked", CityClicked);
        }

        private void MoveActionSelected()
        {
            _moveSelected = true;
            InfoText = "Select city where do you want to move";

            bool canCharterFlight = CurrentCharacter.CanCharterFlight();

            foreach (var city in Board.WorldMap.Cities.Values)
            {
                if (canCharterFlight)
                {
                    city.IsEnabled = true;
                }
                else if (city != CurrentCharacter.MapCity)
                {
                    var enabled = CurrentCharacter.CanDriveOrFerry(city) || CurrentCharacter.CanShuttleFlight(city) || CurrentCharacter.CanDirectFlight(city);
                    city.IsEnabled = enabled;
                }
                else
                {
                    city.IsEnabled = false;
                }
            }
        }

        private void Cancel()
        {
            InfoText = string.Empty;
            _moveSelected = false;
            EnableAllCities();
        }

        private void CardSelected(PlayerCard card)
        {
            if (_moveSelected && _destinationCity != null)
            {
                if (card.City == CurrentCharacter.MapCity.City && CurrentCharacter.CanCharterFlight())
                {
                    CurrentCharacter.CharterFlight(_destinationCity);
                }
                else if (card.City == _destinationCity.City && CurrentCharacter.CanDirectFlight(_destinationCity))
                {
                    CurrentCharacter.DirectFlight(_destinationCity);
                }
            }
        }


        private bool CanDiscoverCure()
        {
            return CurrentCharacter.CanDiscoverCure(Disease.Diseases[DiseaseColor.Yellow]);
        }

        private void DiscoverCure()
        {

        }

        private bool CanBuildStructure()
        {
            return CurrentCharacter.CanBuildResearchStation();
        }

        private void BuildStructure()
        {
            CurrentCharacter.MapCity.HasResearchStation = true;
            var card = CurrentCharacter.Player.RemoveCardWithCity(CurrentCharacter.MapCity.City);
            Board.PlayerDiscardPile.Add(card);
        }

        private void CityClicked(MapCity city)
        {
            if (_moveSelected)
            {
                if (CurrentCharacter.CanDriveOrFerry(city))
                {
                    CurrentCharacter.DriveOrFerry(city);
                }
                else if (CurrentCharacter.CanShuttleFlight(city))
                {
                    CurrentCharacter.ShuttleFlight(city);
                }
                else
                {
                    _destinationCity = city;
                    return;
                }

                _moveSelected = false;
                EnableAllCities();
            }
            else
            {
                PlayerCard card = Board.DrawCard();
                if (card != null)
                {
                    CurrentCharacter.Player.AddCard(card);
                }
            }

            (DiscoverCureActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();

            InfoText = string.Empty;
        }

        private void EnableAllCities()
        {
            foreach (var city in Board.WorldMap.Cities.Values)
            {
                city.IsEnabled = true;
            }
        }
    }
}

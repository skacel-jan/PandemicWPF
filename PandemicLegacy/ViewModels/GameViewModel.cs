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

        public Board Board { get; private set; }

        public int BoardInfectionPosition => Board.InfectionPosition;

        private Character _currentCharacter;
        public Character CurrentCharacter
        {
            get { return _currentCharacter; }
            set { Set(ref _currentCharacter, value); }
        }

        public GameViewModel()
        {
            var mapFactory = new WorldMapFactory();
            var cities = mapFactory.GetCities();

            Board = new Board(mapFactory.BuildMap(), new InfectionDeck(cities), new PlayerDeck(cities));
            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            DiscoverCureActionCommand = new RelayCommand(DiscoverCure, CanDiscoverCure);
            BuildActionCommand = new RelayCommand(BuildStructure, CanBuildStructure);


            CurrentCharacter = new Medic()
            {
                Player = new Player() { Pawn = new Pawn(Colors.White) },
                MapCity = Board.WorldMap.Cities[City.Lagos]
            };

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

            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void CityClicked(MapCity city)
        {
            PlayerCard card = Board.DrawCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            (DiscoverCureActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private bool IsCityEnabled(MapCity city)
        {
            return true;
        }
    }
}

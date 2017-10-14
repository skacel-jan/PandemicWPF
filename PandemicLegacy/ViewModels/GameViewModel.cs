using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PandemicLegacy.Characters;
using PandemicLegacy.Decks;
using System;
using System.Collections.Generic;
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
        public ICommand GenerateCommand { get; set; }
        public ICommand DiseaseSelectedCommand { get; set; }

        public Board Board { get; private set; }

        public int BoardInfectionPosition => Board.InfectionPosition;

        private IList<Character> _characters;
        public IList<Character> Characters
        {
            get { return _characters; }
            set { Set(ref _characters, value); }
        }

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

        private bool _moveSelected;
        public bool MoveSelected
        {
            get => _moveSelected;
            set
            {
                Set(ref _moveSelected, value);
                EnableAllCities();
            }
        }

        private ViewModelBase _infoViewModel;
        public ViewModelBase InfoViewModel
        {
            get { return _infoViewModel; }
            set
            {
                Set(ref _infoViewModel, value);
                IsInfoVisible = value == null ? false : true;
            }
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
            MoveActionCommand = new RelayCommand(MoveActionSelected);
            CancelCommand = new RelayCommand(Cancel);
            //CardCommand = new RelayCommand<PlayerCard>(CardSelected);
            TreatActionCommand = new RelayCommand(SelectDisease, CanTreatDisease);
            GenerateCommand = new RelayCommand(Generate);

            _characters = new List<Character>(4)
            {
                new Medic()
                {
                    Player = new Player() { Pawn = new Pawn(Colors.Brown) },
                    MapCity = Board.WorldMap.Cities[City.Atlanta]
                },
                new Scientist()
                {
                    Player = new Player() { Pawn = new Pawn(Colors.Green) },
                    MapCity = Board.WorldMap.Cities[City.Atlanta]
                }
            };

            CurrentCharacter = _characters[0];

            Board.WorldMap.Cities[City.Atlanta].HasResearchStation = true;

            PlayerCard card = Board.DrawPlayerCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            card = Board.DrawPlayerCard();
            if (card != null)
            {
                CurrentCharacter.Player.AddCard(card);
            }

            MessengerInstance.Register<MapCity>(this, "CityClicked", CityClicked);
        }

        private void Generate()
        {
            //CurrentCharacter.MapCity.BlackInfection++;
            //CurrentCharacter.MapCity.BlueInfection++;
            //CurrentCharacter.MapCity.RedInfection++;
            //CurrentCharacter.MapCity.YellowInfection++;
            DrawEpidemicCard();
        }

        private bool CanTreatDisease()
        {
            return CurrentCharacter.CanTreatDisease();
        }

        private void SelectDisease()
        {
            var list = new List<DiseaseColor>();
            if (CurrentCharacter.MapCity.BlackInfection > 0) list.Add(DiseaseColor.Black);
            if (CurrentCharacter.MapCity.BlueInfection > 0) list.Add(DiseaseColor.Blue);
            if (CurrentCharacter.MapCity.RedInfection > 0) list.Add(DiseaseColor.Red);
            if (CurrentCharacter.MapCity.YellowInfection > 0) list.Add(DiseaseColor.Yellow);

            var diseaseSelectionVM = new DiseaseSelectionViewModel(list);
            diseaseSelectionVM.DiseaseSelected += TreateDisease;

            InfoViewModel = diseaseSelectionVM;
        }

        private void TreateDisease(object sender, DiseaseColor color)
        {
            CurrentCharacter.TreatDisease(Disease.Diseases[color]);
            InfoViewModel = null;
            (TreatActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void MoveActionSelected()
        {
            _moveSelected = true;
            InfoViewModel = new TextViewModel("Select city where do you want to move");

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
            InfoViewModel = null;
            MoveSelected = false;
        }

        private void CardSelected(Card card, MapCity destinationCity)
        {
            if (_moveSelected)
            {
                if (card is PlayerCard playerCard)
                {
                    if (playerCard.City == CurrentCharacter.MapCity.City && CurrentCharacter.CanCharterFlight())
                    {
                        CurrentCharacter.CharterFlight(destinationCity);
                    }
                    else if (playerCard.City == destinationCity.City && CurrentCharacter.CanDirectFlight(destinationCity))
                    {
                        CurrentCharacter.DirectFlight(destinationCity);
                    }
                    OnCharacterMove();
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
            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void CityClicked(MapCity mapCity)
        {
            if (MoveSelected)
            {
                if (CurrentCharacter.CanDriveOrFerry(mapCity))
                {
                    CurrentCharacter.DriveOrFerry(mapCity);
                    OnCharacterMove();
                }
                else if (CurrentCharacter.CanShuttleFlight(mapCity))
                {
                    CurrentCharacter.ShuttleFlight(mapCity);
                    OnCharacterMove();
                }
                else
                {
                    if (CurrentCharacter.CanDirectFlight(mapCity) && CurrentCharacter.CanCharterFlight())
                    {
                        var viewModel = new MoveSelectionViewModel(new List<string>() { "Direct flight", "Charter flight" });
                        InfoViewModel = viewModel;
                    }
                    else
                    {
                        var cardSelectionVM = new CardsSelectionViewModel(CurrentCharacter.Player.Cards, mapCity);
                        cardSelectionVM.CardSelected += (sender, card) => CardSelected(card, mapCity);

                        InfoViewModel = cardSelectionVM;
                    }


                    //_destinationCity = city;
                    //InfoViewModel = new TextViewModel("Select card of destination city");
                    return;
                }
            }
            else
            {
                PlayerCard card = Board.DrawPlayerCard();
                if (card != null)
                {
                    CurrentCharacter.Player.AddCard(card);
                }
            }

            (DiscoverCureActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void OnCharacterMove()
        {
            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (TreatActionCommand as RelayCommand).RaiseCanExecuteChanged();
            MoveSelected = false;
            InfoViewModel = null;
        }

        private void EnableAllCities()
        {
            foreach (var city in Board.WorldMap.Cities.Values)
            {
                city.IsEnabled = true;
            }
        }

        private void DrawEpidemicCard()
        {
            InfectionCard card = Board.DrawInfectionBottomCard();
            var isOutbreak = Board.WorldMap.Cities[card.City.Name].RaiseInfection(card.City.Color);
            if (isOutbreak)
                InfoViewModel = new TextViewModel("Outbreak");
        }
    }
}

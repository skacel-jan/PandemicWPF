using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PandemicLegacy.Characters;
using PandemicLegacy.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICommand InstantMoveCommand { get; set; }

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

        public bool ResearchStationDestroy { get; private set; }

        public Diseases Diseases { get; private set; }
        public MapCity SelectedCity { get; private set; }
        public MoveType? MoveTypeSelected { get; private set; }

        public GameViewModel()
        {
            var mapFactory = new WorldMapFactory();
            var cities = mapFactory.GetCities();

            Diseases = new Diseases();

            Board = new Board(mapFactory.BuildMap(), new InfectionDeck(cities), new PlayerDeck(cities));
            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            DiscoverCureActionCommand = new RelayCommand(SelectCardsToDiscoverCure, CanDiscoverCure);
            BuildActionCommand = new RelayCommand(BuildStructure, CanBuildStructure);
            MoveActionCommand = new RelayCommand(MoveActionSelected);
            CancelCommand = new RelayCommand(Cancel);
            //CardCommand = new RelayCommand<PlayerCard>(CardSelected);
            TreatActionCommand = new RelayCommand(SelectDisease, CanTreatDisease);
            GenerateCommand = new RelayCommand(NextTurn);
            InstantMoveCommand = new RelayCommand<MapCity>(InstantMove);
            ShareActionCommand = new RelayCommand(() => {; });

            InitialInfection();

            _characters = new List<Character>(4)
            {
                new Medic()
                {
                    Player = new Player() { Pawn = new Pawn(Colors.Brown) },
                    CurrentMapCity = Board.WorldMap.Cities[City.Atlanta]
                },
                new Scientist()
                {
                    Player = new Player() { Pawn = new Pawn(Colors.Green) },
                    CurrentMapCity = Board.WorldMap.Cities[City.Atlanta]
                }
            };

            CurrentCharacter = _characters[0];

            Board.BuildResearchStation(Board.WorldMap.GetCity(City.Atlanta));

            foreach (var character in _characters)
            {
                DrawPlayerCards(6 - _characters.Count, character);
            }
            AddEpidemicCards();

            MessengerInstance.Register<MapCity>(this, "CityClicked", CitySelected);
            MessengerInstance.Register<Card>(this, "CardSelection", CardSelected);
            MessengerInstance.Register<DiseaseColor>(this, "DiseaseSelection", TreateDisease);
            MessengerInstance.Register<MoveType>(this, "MoveSelection", MoveSelection);
            MessengerInstance.Register<IList<CityCard>>(this, "CardsSelection", DiscoverCure);
        }

        private void RefreshAllCommands()
        {
            (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (DiscoverCureActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (MoveActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (TreatActionCommand as RelayCommand).RaiseCanExecuteChanged();
            (ShareActionCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void InitialInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                foreach (var x in Enumerable.Range(0, 3))
                {
                    var infectionCard = Board.DrawInfectionCard();
                    Board.WorldMap.GetCity(infectionCard.City.Name).ChangeInfection(infectionCard.City.Color, i);
                    Board.DecreaseCubePile(infectionCard.City.Color, i);
                }
            }
        }

        private void AddEpidemicCards()
        {
            var deckCount = Board.PlayerDeck.Count / 5;
            var deckIncrement = Board.PlayerDeck.Count % 5;

            List<Card> resultDeck = new List<Card>();
            foreach (var i in Enumerable.Range(0, 5))
            {
                var count = deckCount + (deckIncrement > 0 ? 1 : 0);
                var cards = Board.PlayerDeck.Take(count).ToList();
                deckIncrement = deckIncrement == 0 ? 0 : deckIncrement - 1;
                cards.Add(new EpidemicCard());
                resultDeck.AddRange(Deck<Card>.Shuffle(cards));
                Board.PlayerDeck.RemoveRange(0, count);
            }
            Board.PlayerDeck.Clear();
            Board.PlayerDeck.AddRange(resultDeck);
        }

        private void NextTurn()
        {
            DrawPlayerCards(2, CurrentCharacter);
            DrawInfectionCard();
        }

        private void DrawPlayerCards(int count, Character character)
        {
            foreach (var i in Enumerable.Range(0, 2))
            {
                Card card = Board.DrawPlayerCard();

                if (card == null)
                    InfoViewModel = new TextViewModel("Game over");

                if (card is PlayerCard playerCard)
                    character.Player.AddCard(playerCard);
                else if (card is EpidemicCard epidemicCard)
                    InfoViewModel = new TextViewModel("Epidemic");
            }
        }

        private bool CanTreatDisease()
        {
            return CurrentCharacter.CanTreatDisease();
        }

        private void SelectDisease()
        {
            var diseases = new List<DiseaseColor>();
            if (CurrentCharacter.CurrentMapCity.BlackInfection > 0) diseases.Add(DiseaseColor.Black);
            if (CurrentCharacter.CurrentMapCity.BlueInfection > 0) diseases.Add(DiseaseColor.Blue);
            if (CurrentCharacter.CurrentMapCity.RedInfection > 0) diseases.Add(DiseaseColor.Red);
            if (CurrentCharacter.CurrentMapCity.YellowInfection > 0) diseases.Add(DiseaseColor.Yellow);

            InfoViewModel = new DiseaseSelectionViewModel(diseases);
        }

        private void TreateDisease(DiseaseColor color)
        {
            var cubesCount = CurrentCharacter.TreatDisease(Diseases[color]);
            Board.IncreaseCubePile(color, cubesCount);
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
                else if (city != CurrentCharacter.CurrentMapCity)
                {
                    city.IsEnabled = CurrentCharacter.CanDriveOrFerry(city) || CurrentCharacter.CanShuttleFlight(city) || CurrentCharacter.CanDirectFlight(city);
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
            RefreshAllCommands();
        }

        private void CardSelected(Card card)
        {
            if (MoveSelected)
            {
                if (card is PlayerCard playerCard)
                {
                    if ((MoveTypeSelected == MoveType.Direct || MoveTypeSelected == null) && playerCard.City == SelectedCity.City && CurrentCharacter.CanDirectFlight(SelectedCity))
                    {
                        DirectFlightAction();
                    }
                    else if ((MoveTypeSelected == MoveType.Charter || MoveTypeSelected == null) && playerCard.City == CurrentCharacter.CurrentMapCity.City && CurrentCharacter.CanCharterFlight())
                    {
                        CharterFlightAction();
                    }
                    OnCharacterMove();
                }
            }
        }

        private void CharterFlightAction()
        {
            var card = CurrentCharacter.CharterFlight(SelectedCity);
            Board.PlayerDiscardPile.Add(card);
        }

        private void DirectFlightAction()
        {
            var card = CurrentCharacter.DirectFlight(SelectedCity);
            Board.PlayerDiscardPile.Add(card);
        }

        private void MoveSelection(MoveType type)
        {
            MoveTypeSelected = type;
            InfoViewModel = new CardSelectionViewModel(CurrentCharacter.Player.Cards);
        }

        private bool CanDiscoverCure()
        {
            return CurrentCharacter.CanDiscoverCure(Diseases[CurrentCharacter.Player.MostCardsColor]);
        }

        private void DiscoverCure(IList<CityCard> cards)
        {
            CurrentCharacter.DiscoverCure(Diseases[CurrentCharacter.Player.MostCardsColor]);
            foreach (var card in cards)
            {
                CurrentCharacter.Player.RemoveCard(card as PlayerCard);
                Board.PlayerDiscardPile.Add(card);
            }
            InfoViewModel = null;
        }

        private void SelectCardsToDiscoverCure()
        {
            InfoViewModel = new MultiCardsSelectionViewModel(CurrentCharacter.Player.Cards, CurrentCharacter.CardsForCure, Diseases[CurrentCharacter.Player.MostCardsColor].Color);
        }

        private bool CanBuildStructure()
        {
            return CurrentCharacter.CanBuildResearchStation();
        }

        private void BuildStructure()
        {
            if (Board.ResearchStationsPile > 0)
            {
                var card = CurrentCharacter.Player.RemoveCard(CurrentCharacter.CurrentMapCity.City);
                Board.BuildResearchStation(CurrentCharacter.CurrentMapCity, card);
                (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
            }
            else
            {
                InfoViewModel = new TextViewModel("Select a city where do you want to destroy research station");
                ResearchStationDestroy = true;
            }
        }

        private void CitySelected(MapCity mapCity)
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
                    SelectedCity = mapCity;
                    if (CurrentCharacter.CanDirectFlight(mapCity) && CurrentCharacter.CanCharterFlight())
                    {
                        InfoViewModel = new MoveSelectionViewModel(new List<string>() { "Direct flight", "Charter flight" });
                    }
                    else
                    {
                        InfoViewModel = new CardSelectionViewModel(CurrentCharacter.Player.Cards);
                    }
                }
            }
            else if (ResearchStationDestroy)
            {
                Board.DestroyResearchStation(mapCity);
                BuildStructure();
                ResearchStationDestroy = false;

                (DiscoverCureActionCommand as RelayCommand).RaiseCanExecuteChanged();
            }

        }

        private void OnCharacterMove()
        {
            RefreshAllCommands();
            MoveSelected = false;
            MoveTypeSelected = null;
            InfoViewModel = null;
        }

        private void EnableAllCities()
        {
            foreach (var city in Board.WorldMap.Cities.Values)
            {
                city.IsEnabled = true;
            }
        }

        private void DrawInfectionCard()
        {
            InfectionCard card = Board.DrawInfectionCard();
            if (Board.CheckCubesPile(card.City.Color))
            {
                InfoViewModel = new TextViewModel("Game over: no more cubes");
            }
            else
            {
                var isOutbreak = Board.RaiseInfection(card.City);
                if (isOutbreak)
                    InfoViewModel = new TextViewModel("Outbreak");
            }
        }

        private void InstantMove(MapCity mapCity)
        {
            if (CurrentCharacter.CanDriveOrFerry(mapCity))
            {
                CurrentCharacter.DriveOrFerry(mapCity);
                RefreshAllCommands();
            }
        }
    }
}

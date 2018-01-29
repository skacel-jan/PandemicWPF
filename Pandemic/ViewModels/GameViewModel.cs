using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pandemic.Characters;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Pandemic.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public RelayCommand MoveActionCommand { get; set; }
        public RelayCommand TreatActionCommand { get; set; }
        public RelayCommand ShareActionCommand { get; set; }
        public RelayCommand BuildActionCommand { get; set; }
        public RelayCommand DiscoverCureActionCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CardCommand { get; set; }
        public RelayCommand DiseaseSelectedCommand { get; set; }
        public RelayCommand<MapCity> InstantMoveCommand { get; set; }
        public RelayCommand PlayerDiscardPileCommand { get; set; }

        public Board Board { get; private set; }

        private Queue<Character> _characters;
        public Queue<Character> Characters
        {
            get { return _characters; }
            set { Set(ref _characters, value); }
        }

        private Character _currentCharacter;
        public Character CurrentCharacter
        {
            get { return _currentCharacter; }
            set
            {
                if (_currentCharacter != null)
                {
                    _currentCharacter.Player.Pawn.IsActive = false;
                    _currentCharacter.CurrentMapCity.PawnsChanged();
                }
                Set(ref _currentCharacter, value);
                _currentCharacter.Player.Pawn.IsActive = true;
                _currentCharacter.CurrentMapCity.PawnsChanged();
            }
        }

        private bool _isInfoVisible;
        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { Set(ref _isInfoVisible, value); }
        }

        private bool _isMoveSelected;
        public bool IsMoveSelected
        {
            get => _isMoveSelected;
            set
            {
                Set(ref _isMoveSelected, value);
                if (_isMoveSelected == false)
                {
                    EnableAllCities();
                }
            }
        }

        public Card LastCardInPlayerDiscardPile
        {
            get => Board.PlayerDiscardPile.LastOrDefault();
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

        private int _actionsLeft;
        public int ActionsLeft
        {
            get => _actionsLeft;
            private set
            {
                Set(ref _actionsLeft, value);
                if (_actionsLeft == 0)
                {
                    EndOfTurn();
                }
                RaisePropertyChanged(nameof(InfoText));
            }
        }

        public string InfoText
        {
            get { return string.Format("{0}'s turn, {1} actions left", CurrentCharacter.Role, ActionsLeft); }
        }

        public bool ResearchStationDestroy { get; private set; }

        public MapCity SelectedCity { get; private set; }
        public MoveType? MoveTypeSelected { get; private set; }

        public GameViewModel()
        {
            var mapFactory = new WorldMapFactory();
            var cities = mapFactory.GetCities();

            Board = new Board(mapFactory.BuildMap(), new InfectionDeck(cities), new PlayerDeck(cities));
            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            DiscoverCureActionCommand = new RelayCommand(ShowSelecionOfCardsForCure, CanDiscoverCure);
            BuildActionCommand = new RelayCommand(BuildStructure, CanBuildStructure);
            MoveActionCommand = new RelayCommand(MoveActionSelected);
            CancelCommand = new RelayCommand(Cancel);
            TreatActionCommand = new RelayCommand(SelectDisease, CanTreatDisease);
            InstantMoveCommand = new RelayCommand<MapCity>(InstantMove);
            // TODO: Share command
            ShareActionCommand = new RelayCommand(() => {; });

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);

            InitialInfection();

            _characters = new Queue<Character>(
                new Character[] {
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
                }
            );

            SetNextCharacter();

            Board.BuildResearchStation(Board.WorldMap.GetCity(City.Atlanta));

            foreach (var character in _characters)
            {
                DrawPlayerCards(6 - _characters.Count, character);
            }
            AddEpidemicCards();

            MessengerInstance.Register<MapCity>(this, "CityClicked", CitySelected);
            MessengerInstance.Register<Card>(this, "CardSelection", CardSelected);
            MessengerInstance.Register<DiseaseColor>(this, "DiseaseSelection", TreatDisease);
            MessengerInstance.Register<MoveType>(this, "MoveSelection", MoveSelected);
            MessengerInstance.Register<IList<CityCard>>(this, "CardsSelection", DiscoverCure);
        }

        private void ShowPlayerDiscardPile()
        {
            if (InfoViewModel is CardSelectionViewModel)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardSelectionViewModel(Board.PlayerDiscardPile);
            }
        }

        private void AddToPlayerDiscardPile(Card card)
        {
            Board.PlayerDiscardPile.Add(card);
            RaisePropertyChanged(nameof(LastCardInPlayerDiscardPile));
        }

        private void SetNextCharacter()
        {
            CurrentCharacter = _characters.Dequeue();
            ActionsLeft = CurrentCharacter.ActionsCount;
            _characters.Enqueue(CurrentCharacter);
        }

        private void EndOfTurn()
        {
            DrawPlayerCards(2, CurrentCharacter);
            foreach (var i in Enumerable.Range(0, Board.InfectionRate))
            {
                DrawInfectionCard();
            }

            SetNextCharacter();
            RefreshAllCommands();
        }

        private void RefreshAllCommands()
        {
            BuildActionCommand.RaiseCanExecuteChanged();
            DiscoverCureActionCommand.RaiseCanExecuteChanged();
            MoveActionCommand.RaiseCanExecuteChanged();
            TreatActionCommand.RaiseCanExecuteChanged();
            ShareActionCommand.RaiseCanExecuteChanged();
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
                foreach (var card in Board.PlayerDeck.Take(count).ToList())
                {
                    Board.PlayerDeck.Remove(card);
                }

            }
            Board.PlayerDeck.Clear();

            foreach (var card in resultDeck)
            {
                Board.PlayerDeck.Add(card);
            }

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
                {
                    DoEpidemicActions();
                }
            }
        }

        private void DoEpidemicActions()
        {
            InfoViewModel = new TextViewModel("Epidemic");
            Board.RaiseInfectionPosition();
            InfectionCard card = Board.DrawInfectionBottomCard();
            bool isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
            if (isOutbreak)
            {
                DoOutbreak(card.City, Disease.Diseases[card.City.Color]);
            }
            else
            {
                var newDeck = new InfectionDeck(Board.InfectionDiscardPile);
                newDeck.Shuffle();
                foreach (var infectionCard in Board.InfectionDeck)
                {
                    newDeck.Add(infectionCard);
                }
                Board.InfectionDeck = newDeck;
                Board.InfectionDiscardPile.Clear();
            }
        }

        private void DoOutbreak(City city, Disease disease)
        {
            //TODO: outbreaks
            InfoViewModel = new TextViewModel(string.Format("Outbreak in city {0}", city.Name));
            var alreadyOutbreakedCities = new List<City>
            {
                city
            };
            OutbreakCity(city, disease, new List<City>(), alreadyOutbreakedCities);
        }

        private void OutbreakCity(City city, Disease disease, IList<City> citiesToOutbreak, IList<City> alreadyOutbreakedCities)
        {
            var copiedCitiesToOutbreak = new List<City>(citiesToOutbreak);
            foreach (var connectedCity in Board.WorldMap.Cities[city.Name].ConnectedCities)
            {
                bool isOutbreak = Board.RaiseInfection(connectedCity.City, disease.Color);
                if (Board.CheckCubesPile(city.Color))
                {
                    InfoViewModel = new TextViewModel("Game over: no more cubes");
                }
                if (isOutbreak && !alreadyOutbreakedCities.Contains(connectedCity.City))
                {
                    copiedCitiesToOutbreak.Add(connectedCity.City);
                }
            }

            foreach (var outbreakedCity in alreadyOutbreakedCities)
            {
                copiedCitiesToOutbreak.Remove(outbreakedCity);
            }

            if (copiedCitiesToOutbreak.Count > 0)
            {
                foreach (var nextCity in copiedCitiesToOutbreak)
                {
                    alreadyOutbreakedCities.Add(nextCity);
                    OutbreakCity(nextCity, disease, copiedCitiesToOutbreak, alreadyOutbreakedCities);
                }
            }
            else
            {
                return;
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

            if (diseases.Count > 1)
            {
                InfoViewModel = new DiseaseSelectionViewModel(diseases);
            }
            else
            {
                TreatDisease(diseases.First());
            }
        }

        private void TreatDisease(DiseaseColor color)
        {
            var cubesCount = CurrentCharacter.TreatDisease(Disease.Diseases[color]);
            Board.IncreaseCubePile(color, cubesCount);
            InfoViewModel = null;
            DoAction();
            TreatActionCommand.RaiseCanExecuteChanged();
        }

        private void MoveActionSelected()
        {
            IsMoveSelected = true;
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
            IsMoveSelected = false;
            RefreshAllCommands();
        }

        private void CardSelected(Card card)
        {
            if (IsMoveSelected)
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
            AddToPlayerDiscardPile(card);
        }

        private void DirectFlightAction()
        {
            var card = CurrentCharacter.DirectFlight(SelectedCity);
            AddToPlayerDiscardPile(card);
        }

        private void MoveSelected(MoveType type)
        {
            MoveTypeSelected = type;
            InfoViewModel = new CardSelectionViewModel(CurrentCharacter.Player.Cards);
        }

        private bool CanDiscoverCure()
        {
            return CurrentCharacter.CanDiscoverCure(Disease.Diseases[CurrentCharacter.Player.MostCardsColor]);
        }

        private void DiscoverCure(IList<CityCard> cards)
        {
            CurrentCharacter.DiscoverCure(Disease.Diseases[CurrentCharacter.Player.MostCardsColor]);
            foreach (var card in cards)
            {
                CurrentCharacter.Player.RemoveCard(card as PlayerCard);
                AddToPlayerDiscardPile(card);
            }
            InfoViewModel = null;
            DoAction();
        }

        private void ShowSelecionOfCardsForCure()
        {
            InfoViewModel = new MultiCardsSelectionViewModel(CurrentCharacter.Player.Cards, CurrentCharacter.CardsForCure, Disease.Diseases[CurrentCharacter.Player.MostCardsColor].Color);
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
                DoAction();
            }
            else
            {
                InfoViewModel = new TextViewModel("Select a city where do you want to destroy research station");
                ResearchStationDestroy = true;
            }
        }

        private void CitySelected(MapCity mapCity)
        {
            if (IsMoveSelected)
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

                DiscoverCureActionCommand.RaiseCanExecuteChanged();
            }

        }

        private void OnCharacterMove()
        {
            RefreshAllCommands();
            IsMoveSelected = false;
            MoveTypeSelected = null;
            InfoViewModel = null;
            DoAction();
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
                var isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
                if (isOutbreak)
                {
                    DoOutbreak(card.City, Disease.Diseases[card.City.Color]);
                }
            }
        }

        private void InstantMove(MapCity mapCity)
        {
            if (CurrentCharacter.CanDriveOrFerry(mapCity))
            {
                CurrentCharacter.DriveOrFerry(mapCity);
                OnCharacterMove();
            }
        }

        private void DoAction()
        {
            ActionsLeft--;
        }
    }
}

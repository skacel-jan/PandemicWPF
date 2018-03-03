using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private ViewModelBase _infoViewModel;
        private bool _isInfoVisible;
        private bool _isMoveSelected;

        public BoardViewModel(Board board, TurnStateMachine turnStateMachine, ActionStateMachine actionStateMachine)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));

            Board.InfectionDeck.Shuffle();
            Board.PlayerDeck.Shuffle();

            DiscoverCureActionCommand = new RelayCommand(ShowSelecionOfCardsForCure, CanDiscoverCure);
            BuildActionCommand = new RelayCommand(OnBuildStructure, CanBuildStructure);
            MoveActionCommand = new RelayCommand(OnMoveActionSelected);
            CancelCommand = new RelayCommand(Cancel);
            TreatActionCommand = new RelayCommand(OnTreatAction, CanTreatDisease);
            // TODO: Share command
            ShareActionCommand = new RelayCommand(() => { TurnStateMachine.DoAction(); });

            DiscardPileCommand = new RelayCommand<string>(ShowDiscardPile);

            Board.BuildResearchStation(Board.WorldMap.GetCity(City.Atlanta));

            InitialInfection();

            TurnStateMachine = turnStateMachine;
            TurnStateMachine.ActionDone += TurnStateMachine_ActionDone;
            TurnStateMachine.DrawDone += TurnStateMachine_DrawDone;
            TurnStateMachine.PropertyChanged += TurnStateMachine_PropertyChanged;
            TurnStateMachine.Start();

            ActionStateMachine = actionStateMachine;
            ActionStateMachine.AfterTreatDisease += ActionStateMachine_AfterTreatDisease;
            ActionStateMachine.Start();

            foreach (var character in TurnStateMachine.Characters)
            {
                DrawPlayerCards(6 - TurnStateMachine.Characters.Count, character);
            }

            Board.PlayerDeck.AddEpidemicCards(5);

            MessengerInstance.Register<MapCity>(this, Messenger.CitySelected, OnCitySelected);
            MessengerInstance.Register<Card>(this, Messenger.CardSelected, CardSelected);
            MessengerInstance.Register<DiseaseColor>(this, Messenger.DiseaseSelected, (DiseaseColor color) => ActionStateMachine.DoAction(ActionStateMachine.Treat, color));
            MessengerInstance.Register<MoveType>(this, Messenger.MoveSelected, MoveSelected);
            MessengerInstance.Register<IList<CityCard>>(this, Messenger.MultipleCardsSelected, DiscoverCure);
            MessengerInstance.Register<MapCity>(this, Messenger.InstantMove, InstantMove);
        }

        public ActionStateMachine ActionStateMachine { get; }

        public Board Board { get; private set; }

        public RelayCommand BuildActionCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public RelayCommand CardCommand { get; set; }

        public Character CurrentCharacter
        {
            get { return TurnStateMachine.CurrentCharacter; }
        }

        public RelayCommand<string> DiscardPileCommand { get; set; }

        public RelayCommand DiscoverCureActionCommand { get; set; }

        public RelayCommand DiseaseSelectedCommand { get; set; }

        public string InfoText
        {
            get { return string.Format("{0}'s turn, {1} actions left", CurrentCharacter.Role, TurnStateMachine.Actions); }
        }

        public ViewModelBase InfoViewModel
        {
            get { return _infoViewModel; }
            set
            {
                Set(ref _infoViewModel, value);
                IsInfoVisible = value == null ? false : true;
            }
        }

        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { Set(ref _isInfoVisible, value); }
        }

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

        public Card LastCardInInfectionDiscardPile
        {
            get => Board.InfectionDiscardPile.Cards.LastOrDefault();
        }

        public Card LastCardInPlayerDiscardPile
        {
            get => Board.PlayerDiscardPile.Cards.LastOrDefault();
        }

        public RelayCommand MoveActionCommand { get; set; }

        public MoveType? MoveTypeSelected { get; private set; }

        public bool ResearchStationDestroy { get; private set; }

        public MapCity SelectedCity { get; private set; }

        public RelayCommand ShareActionCommand { get; set; }

        public RelayCommand TreatActionCommand { get; set; }

        public TurnStateMachine TurnStateMachine { get; }

        private void ActionStateMachine_AfterTreatDisease(object sender, TreatDiseaseEventArgs e)
        {
            Board.IncreaseCubePile(e.DiseaseColor, e.CubesCount);
            InfoViewModel = null;
            TreatActionCommand.RaiseCanExecuteChanged();
        }

        private void AddToPlayerDiscardPile(Card card)
        {
            Board.PlayerDiscardPile.Cards.Add(card);
            RaisePropertyChanged(nameof(LastCardInPlayerDiscardPile));
        }

        private bool CanBuildStructure()
        {
            return CurrentCharacter.CanBuildResearchStation();
        }

        private void Cancel()
        {
            InfoViewModel = null;
            IsMoveSelected = false;
            RefreshAllCommands();
        }

        private bool CanDiscoverCure()
        {
            return CurrentCharacter.CanDiscoverCure(CurrentCharacter.MostCardsColor);
        }

        private bool CanTreatDisease()
        {
            return CurrentCharacter.DiseasesToTreat() > 0;
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

        private void DirectFlightAction()
        {
            var card = CurrentCharacter.DirectFlight(SelectedCity);
            AddToPlayerDiscardPile(card);
        }

        private void DiscoverCure(IList<CityCard> cards)
        {
            Board.DiscoverCure(CurrentCharacter.MostCardsColor);
            foreach (var card in cards)
            {
                CurrentCharacter.RemoveCard(card as PlayerCard);
                AddToPlayerDiscardPile(card);
            }
            InfoViewModel = null;
        }

        private void DoEpidemicActions()
        {
            InfoViewModel = new TextViewModel("Epidemic");
            Board.RaiseInfectionPosition();
            InfectionCard card = Board.DrawInfectionBottomCard();
            bool isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
            if (isOutbreak)
            {
                Board.Outbreaks++;
                DoOutbreak(card.City, card.City.Color);
            }
            else
            {
                Board.ShuffleDiscardPile();
            }
        }

        private void DoOutbreak(City city, DiseaseColor diseaseColor)
        {
            var citiesToOutbreak = new Queue<City>(1);
            var alreadyOutbreakedCities = new List<City>();
            citiesToOutbreak.Enqueue(city);

            InfoViewModel = new TextViewModel(string.Format("Outbreak in city {0}", city.Name));

            while (citiesToOutbreak.Count > 0)
            {
                var outbreakCity = citiesToOutbreak.Dequeue();
                alreadyOutbreakedCities.Add(outbreakCity);

                foreach (var connectedCity in Board.WorldMap.Cities[outbreakCity.Name].ConnectedCities)
                {
                    bool isOutbreak = Board.RaiseInfection(connectedCity.City, diseaseColor);
                    if (Board.CheckCubesPile(city.Color))
                    {
                        GameOver(10);
                    }

                    if (isOutbreak && !alreadyOutbreakedCities.Contains(connectedCity.City) && !citiesToOutbreak.Contains(connectedCity.City))
                    {
                        citiesToOutbreak.Enqueue(connectedCity.City);
                        Board.Outbreaks++;
                    }
                }
            }
        }

        private void DrawInfectionCard()
        {
            InfectionCard card = Board.DrawInfectionCard();
            if (Board.CheckCubesPile(card.City.Color))
            {
                GameOver(10);
            }
            else
            {
                var isOutbreak = Board.RaiseInfection(card.City, card.City.Color);
                if (isOutbreak)
                {
                    DoOutbreak(card.City, card.City.Color);
                }
            }
        }

        private void DrawPlayerCards(int count, Character character)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                Card card = Board.DrawPlayerCard();

                if (card == null)
                {
                    GameOver(20);
                }

                if (card is PlayerCard playerCard)
                {
                    character.AddCard(playerCard);
                }
                else if (card is EpidemicCard epidemicCard)
                {
                    DoEpidemicActions();
                }
            }
        }

        private void EnableAllCities()
        {
            foreach (var city in Board.WorldMap.Cities.Values)
            {
                city.IsEnabled = true;
            }
        }

        private void EndOfTurn()
        {
            DrawPlayerCards(2, CurrentCharacter);
            foreach (var i in Enumerable.Range(0, Board.InfectionRate))
            {
                DrawInfectionCard();
            }

            RefreshAllCommands();
        }

        private void GameOver(int type)
        {
            if (type == 10)
            {
                InfoViewModel = new TextViewModel("Game over: No more cubes");
            }
            else if (type == 20)
            {
                InfoViewModel = new TextViewModel("Game over: No more cards");
            }
        }

        private void CharterFlightAction()
        {
            var card = CurrentCharacter.CharterFlight(SelectedCity);
            AddToPlayerDiscardPile(card);
        }

        private void InitialInfection()
        {
            for (int i = 3; i > 0; i--)
            {
                foreach (var x in Enumerable.Range(0, 3))
                {
                    var infectionCard = Board.DrawInfectionCard();
                    int changeInfections = Board.WorldMap.GetCity(infectionCard.City.Name).ChangeInfection(infectionCard.City.Color, i);
                    Board.DecreaseCubePile(infectionCard.City.Color, changeInfections);
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

        private void MoveSelected(MoveType type)
        {
            MoveTypeSelected = type;
            InfoViewModel = new CardSelectionViewModel(CurrentCharacter.Cards);
        }

        private void OnBuildStructure()
        {
            if (Board.ResearchStationsPile > 0)
            {
                var card = CurrentCharacter.RemoveCard(CurrentCharacter.CurrentMapCity.City);
                Board.BuildResearchStation(CurrentCharacter.CurrentMapCity, card);
                (BuildActionCommand as RelayCommand).RaiseCanExecuteChanged();
            }
            else
            {
                InfoViewModel = new TextViewModel("Select a city where do you want to destroy research station");
                ResearchStationDestroy = true;
            }
        }

        private void OnCitySelected(MapCity mapCity)
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
                        InfoViewModel = new CardSelectionViewModel(CurrentCharacter.Cards);
                    }
                }
            }
            else if (ResearchStationDestroy)
            {
                Board.DestroyResearchStation(mapCity);
                OnBuildStructure();
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
            TurnStateMachine.DoAction();
        }

        private void OnMoveActionSelected()
        {
            IsMoveSelected = true;
            InfoViewModel = new TextViewModel("Select city where do you want to move");
            Task.Run(() =>
            {
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
            });
        }

        private void OnTreatAction()
        {
            ActionStateMachine.DoAction(ActionStateMachine.Treat);
        }

        private void RefreshAllCommands()
        {
            BuildActionCommand.RaiseCanExecuteChanged();
            DiscoverCureActionCommand.RaiseCanExecuteChanged();
            MoveActionCommand.RaiseCanExecuteChanged();
            TreatActionCommand.RaiseCanExecuteChanged();
            ShareActionCommand.RaiseCanExecuteChanged();
        }

        private void ShowDiscardPile(string pileType)
        {
            if (pileType == "Infection")
            {
                if (InfoViewModel is CardSelectionViewModel)
                {
                    InfoViewModel = null;
                }
                else
                {
                    InfoViewModel = new CardSelectionViewModel(Board.InfectionDiscardPile.Cards);
                }
            }
            else if (pileType == "Player")
            {
                if (InfoViewModel is CardSelectionViewModel)
                {
                    InfoViewModel = null;
                }
                else
                {
                    InfoViewModel = new CardSelectionViewModel(Board.PlayerDiscardPile.Cards);
                }
            }
        }

        private void ShowSelecionOfCardsForCure()
        {
            InfoViewModel = new MultiCardsSelectionViewModel(CurrentCharacter.Cards, CurrentCharacter.CardsForCure, CurrentCharacter.MostCardsColor);
        }

        private void TurnStateMachine_ActionDone(object sender, GenericEventArgs<int> e)
        {
        }

        private void TurnStateMachine_DrawDone(object sender, GenericEventArgs<int> e)
        {
            DrawPlayerCards(1, CurrentCharacter);
            if (e.EventData > 0)
            {
                InfoViewModel = new TextViewModel(CurrentCharacter.Cards.Last().Name, ShareActionCommand, "Draw next card");
            }
            else
            {
                InfoViewModel = new TextViewModel(CurrentCharacter.Cards.Last().Name, new RelayCommand(() => InfoViewModel = null), "Infection phase");
            }
        }

        private void TurnStateMachine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TurnStateMachine.CurrentCharacter))
            {
                ActionStateMachine.CurrentCharacter = TurnStateMachine.CurrentCharacter;
            }
        }
    }
}
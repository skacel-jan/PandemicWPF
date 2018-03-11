using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private ViewModelBase _actionViewModel;
        private ViewModelBase _infoViewModel;
        private bool _isActionVisible;
        private bool _isDiscardingSelected;
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

            ActionStateMachine = actionStateMachine;
            ActionStateMachine.AfterTreatDisease += ActionStateMachine_AfterTreatDisease;
            ActionStateMachine.Start();

            TurnStateMachine = turnStateMachine;
            TurnStateMachine.ActionDone += TurnStateMachine_ActionDone;
            TurnStateMachine.DrawDone += TurnStateMachine_DrawDone;
            TurnStateMachine.DoInfection += TurnStateMachine_InfectionDone;
            TurnStateMachine.ActionPhaseEnded += TurnStateMachine_ActionPhaseEnded;
            TurnStateMachine.DrawingPhaseEnded += TurnStateMachine_DrawingPhaseEnded;
            TurnStateMachine.InfectionPhaseEnded += TurnStateMachine_InfectionPhaseEnded;
            TurnStateMachine.TurnStarted += TurnStateMachine_TurnStarted; ;
            TurnStateMachine.Start();

            foreach (var character in TurnStateMachine.Characters)
            {
                DrawPlayerCards(6 - TurnStateMachine.Characters.Count, character);
            }

            Board.PlayerDeck.AddEpidemicCards(5);

            MessengerInstance.Register<GenericMessage<MapCity>>(this, Messenger.CitySelected, MoveCharacterToCity);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, Messenger.ResearchStation, DestroyResearchStation);
            MessengerInstance.Register<GenericMessage<Card>>(this, Messenger.MoveAction, CardSelected);
            MessengerInstance.Register<DiseaseColor>(this, (DiseaseColor color) => ActionStateMachine.DoAction(ActionStateMachine.Treat, color));
            MessengerInstance.Register<MoveType>(this, MoveSelected);
            MessengerInstance.Register<IList<CityCard>>(this, DiscoverCure);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, Messenger.InstantMove, InstantMove);
            MessengerInstance.Register<GenericMessage<Card>>(this, Messenger.DiscardAction, DiscardCard);
        }

        public ActionStateMachine ActionStateMachine { get; }

        public ViewModelBase ActionViewModel
        {
            get { return _actionViewModel; }
            set
            {
                Set(ref _actionViewModel, value);
                IsActionVisible = value == null ? false : true;
            }
        }

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

        public bool IsActionVisible
        {
            get { return _isActionVisible; }
            set { Set(ref _isActionVisible, value); }
        }

        public bool IsDiscardingSelected
        {
            get => _isDiscardingSelected;
            set => Set(ref _isDiscardingSelected, value);
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
                    ResetMoteToAllCities();
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
            ActionViewModel = null;
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

        private void CardSelected(GenericMessage<Card> cardMessage)
        {
            if (cardMessage.Content is PlayerCard playerCard)
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

        private void DestroyResearchStation(GenericMessage<MapCity> cityMessage)
        {
            Board.DestroyResearchStation(cityMessage.Content);
            OnBuildStructure();

            DiscoverCureActionCommand.RaiseCanExecuteChanged();
        }

        private void DirectFlightAction()
        {
            var card = CurrentCharacter.DirectFlight(SelectedCity);
            AddToPlayerDiscardPile(card);
        }

        private void DiscardCard(GenericMessage<Card> cardMessage)
        {
            CurrentCharacter.RemoveCard(cardMessage.Content as PlayerCard);

            if (CurrentCharacter.HasMoreCardsThenLimit)
            {
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}.{2}Player has more cards then his hand limit. Card has to be discarded.",
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name, Environment.NewLine));
                IsDiscardingSelected = true;
                ActionViewModel = new CardSelectionViewModel(CurrentCharacter.Cards, Messenger.DiscardAction);
            }
            else
            {
                ActionViewModel = null;
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}",
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name),
                    new RelayCommand(() => TurnStateMachine.DoAction()), "Go to Infection phase");
            }
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
            ActionViewModel = new TextViewModel("Epidemic");
            Board.RaiseInfectionPosition();
            TurnStateMachine.InfectionsRate = Board.InfectionRate;
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

        private InfectionCard DrawInfectionCard()
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

            return card;
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

        private void InstantMove(GenericMessage<MapCity> mapCityMessage)
        {
            if (CurrentCharacter.CanDriveOrFerry(mapCityMessage.Content))
            {
                CurrentCharacter.DriveOrFerry(mapCityMessage.Content);
                OnCharacterMove();
            }
        }

        private void MoveCharacterToCity(GenericMessage<MapCity> mapCityMessage)
        {
            var mapCity = mapCityMessage.Content;

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
                    ActionViewModel = new MoveSelectionViewModel(new List<string>() { "Direct flight", "Charter flight" });
                }
                else
                {
                    ActionViewModel = new CardSelectionViewModel(CurrentCharacter.Cards, Messenger.MoveAction);
                }
            }
        }

        private void MoveSelected(MoveType type)
        {
            MoveTypeSelected = type;
            ActionViewModel = new CardSelectionViewModel(CurrentCharacter.Cards, Messenger.MoveAction);
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

        private void OnCharacterMove()
        {
            RefreshAllCommands();

            IsMoveSelected = false;
            MoveTypeSelected = null;
            InfoViewModel = null;
            ActionViewModel = null;
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
                        city.IsMoveEnabled = true;
                    }
                    else if (city != CurrentCharacter.CurrentMapCity)
                    {
                        city.IsMoveEnabled = CurrentCharacter.CanDriveOrFerry(city) || CurrentCharacter.CanShuttleFlight(city) || CurrentCharacter.CanDirectFlight(city);
                    }
                    else
                    {
                        city.IsMoveEnabled = false;
                    }
                }
            });
        }

        private void OnTreatAction()
        {
            ActionStateMachine.DoAction(ActionStateMachine.Treat);
            TurnStateMachine.DoAction();
        }

        private void RefreshAllCommands()
        {
            BuildActionCommand.RaiseCanExecuteChanged();
            DiscoverCureActionCommand.RaiseCanExecuteChanged();
            MoveActionCommand.RaiseCanExecuteChanged();
            TreatActionCommand.RaiseCanExecuteChanged();
            ShareActionCommand.RaiseCanExecuteChanged();
        }

        private void ResetMoteToAllCities()
        {
            Task.Run(() =>
            {
                foreach (var city in Board.WorldMap.Cities.Values)
                {
                    city.IsMoveEnabled = false;
                }
            });
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
                    InfoViewModel = new CardSelectionViewModel(Board.InfectionDiscardPile.Cards, string.Empty);
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
                    InfoViewModel = new CardSelectionViewModel(Board.PlayerDiscardPile.Cards, string.Empty);
                }
            }
        }

        private void ShowSelecionOfCardsForCure()
        {
            InfoViewModel = new MultiCardsSelectionViewModel(CurrentCharacter.Cards, CurrentCharacter.CardsForCure, CurrentCharacter.MostCardsColor);
        }

        private void TurnStateMachine_ActionDone(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(InfoText));
        }

        private void TurnStateMachine_ActionPhaseEnded(object sender, EventArgs e)
        {
            InfoViewModel = new TextViewModel("Action phase has ended.", new RelayCommand(() => TurnStateMachine.DoAction()),
                "Continue to drawing phase");
        }

        private void TurnStateMachine_DrawDone(object sender, EventArgs e)
        {
            DrawPlayerCards(2, CurrentCharacter);
        }

        private void TurnStateMachine_DrawingPhaseEnded(object sender, EventArgs e)
        {
            if (CurrentCharacter.HasMoreCardsThenLimit)
            {
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}.{2}Player has more cards then his hand limit. Card has to be discarded.",
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name, Environment.NewLine));

                IsDiscardingSelected = true;
                ActionViewModel = new CardSelectionViewModel(CurrentCharacter.Cards, Messenger.DiscardAction);
            }
            else
            {
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}",
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name),
                    new RelayCommand(() => TurnStateMachine.DoAction()), "Go to Infection phase");
            }
        }

        private void TurnStateMachine_InfectionDone(object sender, GenericEventArgs<int> e)
        {
            InfectionCard infectionCard = DrawInfectionCard();
            InfoViewModel = new TextViewModel(string.Format("Infected city: {0}", infectionCard.ToString()),
                new RelayCommand(() => TurnStateMachine.DoAction()), "Next infected city");
        }

        private void TurnStateMachine_InfectionPhaseEnded(object sender, EventArgs e)
        {
            var textViewModel = InfoViewModel as TextViewModel;
            textViewModel.CommandText = "Next player";
        }

        private void TurnStateMachine_TurnStarted(object sender, EventArgs e)
        {
            ActionStateMachine.CurrentCharacter = TurnStateMachine.CurrentCharacter;
            InfoViewModel = null;
            ActionViewModel = null;
            RaisePropertyChanged(nameof(InfoText));
            RaisePropertyChanged(nameof(CurrentCharacter));
            RefreshAllCommands();
        }
    }
}
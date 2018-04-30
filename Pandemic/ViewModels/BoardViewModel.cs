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
        private string _actionEvent;
        private ViewModelBase _actionViewModel;
        private Action<MapCity> _build;
        private ViewModelBase _infoViewModel;
        private bool _isActionVisible;
        private bool _isInfoVisible;

        public BoardViewModel(Board board, TurnStateMachine turnStateMachine, ActionStateMachine actionStateMachine)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));

            ActionStateMachine = actionStateMachine;
            ActionStateMachine.CardsSelecting += ActionStateMachine_CardsSelecting;
            ActionStateMachine.DiseaseSelecting += ActionStateMachine_DiseaseSelecting;
            ActionStateMachine.ShareTypeSelecting += ActionStateMachine_ShareTypeSelecting;
            ActionStateMachine.CharacterSelecting += ActionStateMachine_CharacterSelecting;
            ActionStateMachine.MoveTypeSelecting += ActionStateMachine_MoveTypeSelecting;
            ActionStateMachine.ActionDone += ActionStateMachine_ActionDone;
            ActionStateMachine.EventDone += ActionStateMachine_EventDone;
            ActionStateMachine.CitySelecting += ActionStateMachine_CitySelecting;

            TurnStateMachine = turnStateMachine;
            TurnStateMachine.ActionDone += TurnStateMachine_ActionDone;
            TurnStateMachine.InfectionDone += TurnStateMachine_InfectionDone;
            TurnStateMachine.ActionPhaseEnded += TurnStateMachine_ActionPhaseEnded;
            TurnStateMachine.DrawingPhaseEnded += TurnStateMachine_DrawingPhaseEnded;
            TurnStateMachine.InfectionPhaseEnded += TurnStateMachine_InfectionPhaseEnded;
            TurnStateMachine.TurnStarted += TurnStateMachine_TurnStarted;
            TurnStateMachine.GameLost += TurnStateMachine_GameLost;

            MoveActionCommand = new RelayCommand(OnMoveAction);
            TreatActionCommand = new RelayCommand(() => DoAction(ActionTypes.Treat), () => CurrentCharacter.CanTreatDisease());
            ShareActionCommand = new RelayCommand(() => DoAction(ActionTypes.Share), () =>
            {
                if (CurrentCharacter.CurrentMapCity.Characters.Count() < 2)
                {
                    return false;
                }
                else
                {
                    bool canShare = false;
                    foreach (var character in CurrentCharacter.CurrentMapCity.Characters)
                    {
                        canShare = canShare || character.ShareKnowledgeBehaviour.IsPossible();
                    }
                    return canShare;
                }
            });
            BuildActionCommand = new RelayCommand(() => DoAction(ActionTypes.Build),
                () => CurrentCharacter.BuildBehaviour.CanBuild(CurrentCharacter.CurrentMapCity));
            DiscoverCureActionCommand = new RelayCommand(() => DoAction(ActionTypes.Discover), () => CurrentCharacter.CanDiscoverCure());

            CancelCommand = new RelayCommand(Cancel);

            EventsCommand = new RelayCommand(() => DoAction(ActionTypes.Event));

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);
            InfectionDiscardPileCommand = new RelayCommand(ShowInfectionDiscardPile);

            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.CitySelected, CitySelected);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.InstantMove, InstantMove);

            ActionStateMachine.Start();
            TurnStateMachine.Start();
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

        public RelayCommand BuildActionCommand { get; }

        public RelayCommand CancelCommand { get; }

        public Character CurrentCharacter
        {
            get { return TurnStateMachine.Characters.Current; }
        }

        public RelayCommand DiscoverCureActionCommand { get; }

        public RelayCommand DiseaseSelectedCommand { get; }

        public RelayCommand EventsCommand { get; }

        public RelayCommand InfectionDiscardPileCommand { get; private set; }

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

        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { Set(ref _isInfoVisible, value); }
        }

        public RelayCommand MoveActionCommand { get; set; }

        public RelayCommand PlayerDiscardPileCommand { get; set; }

        public RelayCommand ShareActionCommand { get; set; }

        public RelayCommand TreatActionCommand { get; set; }

        public TurnStateMachine TurnStateMachine { get; }

        private void ActionStateMachine_ActionDone(object sender, EventArgs e)
        {
            FinishDoingAction();
        }

        private void ActionStateMachine_CardsSelecting(object sender, CardsSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);

            ActionViewModel = new CardsSelectionViewModel(e.Cards, (Card c) =>
            {
                e.SelectionDelegate(c);
                HideActionViews();
            });
        }

        private void ActionStateMachine_CitySelecting(object sender, CitySelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            _build = e.SelectionDelegate;
        }

        private void ActionStateMachine_DiseaseSelecting(object sender, EventArgs e)
        {
            ActionViewModel = new DiseaseSelectionViewModel(new List<DiseaseColor>(CurrentCharacter.CurrentMapCity.DiseasesToTreat),
                (disease) => ActionStateMachine.DoAction(_actionEvent, disease));
        }

        private void ActionStateMachine_EventDone(object sender, EventArgs e)
        {
            InfoViewModel = null;
            ActionViewModel = null;

            RefreshAllCommands();
        }

        private void ActionStateMachine_CharacterSelecting(object sender, CharacterSelectingEventArgs e)
        {
            ActionViewModel = new CharacterSelectionViewModel(e.Characters, e.SelectionDelegate);
        }

        private void ActionStateMachine_MoveTypeSelecting(object sender, MoveTypeEventArgs e)
        {
            ActionViewModel = new MoveSelectionViewModel(e.Moves, e.SelectionDelegate);
        }

        private void ActionStateMachine_ShareTypeSelecting(object sender, ShareTypeEventArgs e)
        {
            ActionViewModel = new ShareTypeSelectionViewModel(e.ShareTypes, e.SelectionDelegate);
        }

        private void Cancel()
        {
            if (TurnStateMachine.IsActionPhase())
            {
                InfoViewModel = null;
                ActionViewModel = null;
                _actionEvent = null;
                ActionStateMachine.DoAction(ActionTypes.Cancel);
                RefreshAllCommands();

                Task.Run(() =>
                {
                    foreach (var city in Board.WorldMap.Cities.Values)
                    {
                        city.IsSelectable = false;
                    }
                });
            }
        }

        private void HideActionViews()
        {
            InfoViewModel = null;
            ActionViewModel = null;
        }

        private void CitySelected(GenericMessage<MapCity> mapCityMessage)
        {
            if (_build != null)
            {
                _build.Invoke(mapCityMessage.Content);
            }

            if (!string.IsNullOrEmpty(_actionEvent))
            {
                ActionStateMachine.DoAction(_actionEvent, mapCityMessage.Content);
            }
        }

        private void DoAction(string actionString)
        {
            if (TurnStateMachine.IsActionPhase())
            {
                _actionEvent = actionString;
                ActionStateMachine.DoAction(_actionEvent);
            }
        }

        private void DoAction(string actionString, object parameter)
        {
            if (TurnStateMachine.IsActionPhase())
            {
                _actionEvent = actionString;
                ActionStateMachine.DoAction(actionString, parameter);
            }
        }

        private void FinishDoingAction()
        {
            _actionEvent = null;
            InfoViewModel = null;
            ActionViewModel = null;
            TurnStateMachine.DoAction();
            RefreshAllCommands();
        }

        private void InstantMove(GenericMessage<MapCity> mapCityMessage)
        {
            DoAction(ActionTypes.DriveOrFerry, mapCityMessage.Content);
        }

        private void OnMoveAction()
        {
            if (TurnStateMachine.IsActionPhase())
            {
                InfoViewModel = new TextViewModel("Select city where do you want to move");
                DoAction(ActionTypes.Move);
            }
        }

        private void RefreshAllCommands()
        {
            BuildActionCommand.RaiseCanExecuteChanged();
            DiscoverCureActionCommand.RaiseCanExecuteChanged();
            MoveActionCommand.RaiseCanExecuteChanged();
            TreatActionCommand.RaiseCanExecuteChanged();
            ShareActionCommand.RaiseCanExecuteChanged();
        }

        private void ShowInfectionDiscardPile()
        {
            if (InfoViewModel != null && InfoViewModel is CardsViewModel cardsViewModel && cardsViewModel.Code.Equals("Infection"))
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardsViewModel("Infection", Board.InfectionDiscardPile.Cards);
            }
        }

        private void ShowPlayerDiscardPile()
        {
            if (InfoViewModel != null && InfoViewModel is CardsViewModel cardsViewModel && cardsViewModel.Code.Equals("Player"))
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardsViewModel("Player", Board.PlayerDiscardPile.Cards);
            }
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

        private void TurnStateMachine_DrawingPhaseEnded(object sender, EventArgs e)
        {
            if (CurrentCharacter.HasMoreCardsThenLimit)
            {
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}.{2}Player has more cards then his hand limit. Card has to be discarded.",
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                    CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name, Environment.NewLine));
                IList<Card> selectedCards = new List<Card>(2);
                ActionViewModel = new CardsSelectionViewModel(CurrentCharacter.Cards, (Card card) =>
                {
                    if (selectedCards.Contains(card))
                    {
                        selectedCards.Remove(card);
                    }
                    else
                    {
                        selectedCards.Add(card);
                    }

                    if (CurrentCharacter.CardsLimit == (CurrentCharacter.Cards.Count - selectedCards.Count))
                    {
                        foreach (var playerCard in selectedCards)
                        {
                            CurrentCharacter.RemoveCard(playerCard);
                            TurnStateMachine.DoAction();
                        }
                    }
                });
            }
            else
            {
                InfoViewModel = new TextViewModel(string.Format("Drawn cards: {0} and {1}",
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 1].Name,
                        CurrentCharacter.Cards[CurrentCharacter.Cards.Count - 2].Name),
                    new RelayCommand(() => TurnStateMachine.DoAction()), "Go to Infection phase");
            }
        }

        private void TurnStateMachine_GameLost(object sender, GenericEventArgs<string> e)
        {
            ActionViewModel = null;
            InfoViewModel = new TextViewModel(e.EventData);
        }

        private void TurnStateMachine_InfectionDone(object sender, InfectionEventArgs e)
        {
            InfoViewModel = new TextViewModel(string.Format("Infected city: {0}", e.City),
                new RelayCommand(() => TurnStateMachine.DoAction()), "Next infected city");
        }

        private void TurnStateMachine_InfectionPhaseEnded(object sender, EventArgs e)
        {
            var textViewModel = InfoViewModel as TextViewModel;
            textViewModel.CommandText = "Next player";
        }

        private void TurnStateMachine_TurnStarted(object sender, EventArgs e)
        {
            ActionStateMachine.CurrentCharacter = TurnStateMachine.Characters.Current;
            InfoViewModel = null;
            ActionViewModel = null;
            RaisePropertyChanged(nameof(InfoText));
            RaisePropertyChanged(nameof(CurrentCharacter));
            RefreshAllCommands();
        }
    }
}
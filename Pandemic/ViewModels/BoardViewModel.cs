using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Pandemic.Cards;
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
        private ViewModelBase _infoViewModel;
        private bool _isActionVisible;
        private bool _isInfoVisible;

        public BoardViewModel(Game game, ActionStateMachine actionStateMachine)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));

            ActionStateMachine = actionStateMachine;
            ActionStateMachine.MoveTypeSelecting += ActionStateMachine_MoveTypeSelecting;
            ActionStateMachine.ActionDone += ActionStateMachine_ActionDone;
            ActionStateMachine.EventDone += ActionStateMachine_EventDone;
            ActionStateMachine.CitySelecting += ActionStateMachine_CitySelecting;

            Game.InfoChanged += Game_InfoChanged;
            Game.ActionDone += Game_ActionDone;
            Game.CharacterChanged += Game_CharacterChanged;
            Game.DiseaseSelecting += Game_DiseaseSelecting;
            Game.ShareTypeSelecting += Game_ShareTypeSelecting;
            Game.CharacterSelecting += Game_CharacterSelecting;
            Game.CardSelecting += Game_CardsSelecting;
            //TurnStateMachine.ActionDone += TurnStateMachine_ActionDone;
            //TurnStateMachine.InfectionDone += TurnStateMachine_InfectionDone;
            //TurnStateMachine.ActionPhaseEnded += TurnStateMachine_ActionPhaseEnded;
            //TurnStateMachine.DrawingPhaseEnded += TurnStateMachine_DrawingPhaseEnded;
            //TurnStateMachine.InfectionPhaseEnded += TurnStateMachine_InfectionPhaseEnded;
            //TurnStateMachine.TurnStarted += TurnStateMachine_TurnStarted;
            //TurnStateMachine.GameLost += TurnStateMachine_GameLost;

            MoveActionCommand = new RelayCommand(OnMoveAction);
            TreatActionCommand = new RelayCommand(() => DoAction(ActionTypes.Treat), () => CurrentCharacter.CanTreatDisease(Game));
            ShareActionCommand = new RelayCommand(() => DoAction(ActionTypes.Share), () => CurrentCharacter.CurrentMapCity.Characters.Any(c => c.CanShareKnowledge(Game)));
            BuildActionCommand = new RelayCommand(() => DoAction(ActionTypes.Build), () => CurrentCharacter.CanBuild(Game));
            DiscoverCureActionCommand = new RelayCommand(() => DoAction(ActionTypes.Discover), () => CurrentCharacter.CanDiscoverCure(Game));

            CancelCommand = new RelayCommand(Cancel);

            EventsCommand = new RelayCommand(() =>
            {
                if (ActionViewModel is CardsSelectionViewModel)
                {
                    ActionViewModel = null;
                }
                else
                {
                    DoAction(ActionTypes.Event);
                }


            }, () => Game.EventCards.Count > 0);

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);
            InfectionDiscardPileCommand = new RelayCommand(ShowInfectionDiscardPile);

            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.CitySelected, CitySelected);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.InstantMove, InstantMove);

            ActionStateMachine.Start();
        }

        private void Game_ShareTypeSelecting(object sender, ShareTypeSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            ActionViewModel = new ShareTypeSelectionViewModel(e.ShareTypes, (ShareType type) =>
            {
                HideActionViews();
                e.SelectionDelegate(type);
            });
        }

        private void Game_DiseaseSelecting(object sender, DiseaseSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            ActionViewModel = new DiseaseSelectionViewModel(e.Diseases, (DiseaseColor color) =>
            {
                HideActionViews();
                e.SelectionDelegate(color);
            });
        }

        private void Game_CharacterChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(CurrentCharacter));
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

        public RelayCommand BuildActionCommand { get; }

        public RelayCommand CancelCommand { get; }

        public Character CurrentCharacter
        {
            get { return Game.Characters.Current; }
        }

        public RelayCommand DiscoverCureActionCommand { get; }

        public RelayCommand DiseaseSelectedCommand { get; }

        public RelayCommand EventsCommand { get; }

        public Game Game { get; }

        public RelayCommand InfectionDiscardPileCommand { get; private set; }

        public string InfoText
        {
            get { return string.Format("{0}'s turn, {1} actions left", CurrentCharacter.Role, Game.Actions); }
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

        private void ActionStateMachine_ActionDone(object sender, EventArgs e)
        {
            FinishDoingAction();
        }

        private void Game_CardsSelecting(object sender, CardsSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);

            ActionViewModel = new CardsSelectionViewModel(e.Cards, (Card c) =>
            {
                //HideActionViews();
                e.SelectionDelegate(c);
            });
        }

        private void ActionStateMachine_CitySelecting(object sender, CitySelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
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
            _actionEvent = null;

            RefreshAllCommands();
        }

        private void Game_CharacterSelecting(object sender, CharacterSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            ActionViewModel = new CharacterSelectionViewModel(e.Characters, (Character c) =>
            {
                HideActionViews();
                e.SelectionDelegate(c);
            });
        }

        private void ActionStateMachine_MoveTypeSelecting(object sender, MoveTypeEventArgs e)
        {
            ActionViewModel = new MoveSelectionViewModel(e.Moves, e.SelectionDelegate);
        }

        private void ActionStateMachine_ShareTypeSelecting(object sender, ShareTypeSelectingEventArgs e)
        {
            ActionViewModel = new ShareTypeSelectionViewModel(e.ShareTypes, e.SelectionDelegate);
        }

        private void Cancel()
        {
            InfoViewModel = null;
            ActionViewModel = null;
            _actionEvent = null;
            ActionStateMachine.DoAction(ActionTypes.Cancel);
            RefreshAllCommands();

            Task.Run(() =>
            {
                foreach (var city in Game.WorldMap.Cities.Values)
                {
                    city.IsSelectable = false;
                }
            });
        }

        private void CitySelected(GenericMessage<MapCity> mapCityMessage)
        {
            if (!string.IsNullOrEmpty(_actionEvent))
            {
                ActionStateMachine.DoAction(_actionEvent, mapCityMessage.Content);
            }
        }

        private void DoAction(string actionString)
        {
            _actionEvent = actionString;
            ActionStateMachine.DoAction(_actionEvent);
            Game.DoAction(actionString);
        }

        private void DoAction(string actionString, object parameter)
        {
            _actionEvent = actionString;
            ActionStateMachine.DoAction(actionString, parameter);
            Game.DoAction(actionString);
        }

        private void FinishDoingAction()
        {
            _actionEvent = null;
            InfoViewModel = null;
            ActionViewModel = null;

            RefreshAllCommands();
        }

        private void Game_ActionDone(object sender, EventArgs e)
        {
            RefreshAllCommands();
        }

        private void Game_InfoChanged(object sender, EventArgs e)
        {
            if (Game.Info == null)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new TextViewModel(Game.Info.Text, Game.Info.Action, Game.Info.ButtonText);
            }
        }

        private void HideActionViews()
        {
            InfoViewModel = null;
            ActionViewModel = null;
        }

        private void InstantMove(GenericMessage<MapCity> mapCityMessage)
        {
            DoAction(ActionTypes.DriveOrFerry, mapCityMessage.Content);
        }

        private void OnMoveAction()
        {
            InfoViewModel = new TextViewModel("Select city where do you want to move");
            DoAction(ActionTypes.Move);
        }

        private void RefreshAllCommands()
        {
            BuildActionCommand.RaiseCanExecuteChanged();
            DiscoverCureActionCommand.RaiseCanExecuteChanged();
            MoveActionCommand.RaiseCanExecuteChanged();
            TreatActionCommand.RaiseCanExecuteChanged();
            ShareActionCommand.RaiseCanExecuteChanged();
            EventsCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(InfoText));
        }

        private void ShowInfectionDiscardPile()
        {
            if (InfoViewModel != null && InfoViewModel is CardsViewModel cardsViewModel && cardsViewModel.Code.Equals("Infection"))
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardsViewModel("Infection", Game.InfectionDiscardPile.Cards);
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
                InfoViewModel = new CardsViewModel("Player", Game.PlayerDiscardPile.Cards);
            }
        }

        private void TurnStateMachine_GameLost(object sender, GenericEventArgs<string> e)
        {
            ActionViewModel = null;
            InfoViewModel = new TextViewModel(e.EventData);
        }
    }
}
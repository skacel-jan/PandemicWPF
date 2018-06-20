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
        private ViewModelBase _actionViewModel;
        private ViewModelBase _infoViewModel;
        private bool _isActionVisible;
        private bool _isInfoVisible;

        public BoardViewModel(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));

            Game.InfoChanged += Game_InfoChanged;
            Game.ActionDone += Game_ActionDone;
            Game.CharacterChanged += Game_CharacterChanged;
            Game.DiseaseSelecting += Game_DiseaseSelecting;
            Game.ShareTypeSelecting += Game_ShareTypeSelecting;
            Game.CharacterSelecting += Game_CharacterSelecting;
            Game.CardSelecting += Game_CardsSelecting;

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

            //MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.CitySelected, CitySelected);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.InstantMove, InstantMove);
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

        private void Game_CharacterSelecting(object sender, CharacterSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            ActionViewModel = new CharacterSelectionViewModel(e.Characters, (Character c) =>
            {
                HideActionViews();
                e.SelectionDelegate(c);
            });
        }

        private void Cancel()
        {
            InfoViewModel = null;
            ActionViewModel = null;
            RefreshAllCommands();

            Task.Run(() =>
            {
                foreach (var city in Game.WorldMap.Cities.Values)
                {
                    city.IsSelectable = false;
                }
            });
        }

        private void DoAction(string actionString)
        {
            Game.DoAction(actionString);
        }

        private void FinishDoingAction()
        {
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
            //Game.CurrentCharacter.Actions[ActionTypes.DriveOrFerry]., mapCityMessage.Content);
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
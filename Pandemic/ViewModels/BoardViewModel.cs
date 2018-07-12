using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Pandemic.Cards;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
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
        private Character _currentCharacter;
        private IList<Tuple<RelayCommand, IGameAction>> _actions;
        private bool _isPlayerHandVisible = true;

        public IList<Tuple<RelayCommand, IGameAction>> Actions { get => _actions; set => Set(ref _actions, value); }

        public BoardViewModel(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            CurrentCharacter = Game.CurrentCharacter;

            Game.InfoChanged += Game_InfoChanged;
            Game.ActionDone += Game_ActionDone;
            Game.CharacterChanged += Game_CharacterChanged;
            Game.DiseaseSelecting += Game_DiseaseSelecting;
            Game.ShareTypeSelecting += Game_ShareTypeSelecting;
            Game.CharacterSelecting += Game_CharacterSelecting;
            Game.CardSelecting += Game_CardsSelecting;
            Game.GameEnd += Game_GameEnd;

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
        }

        private void Game_GameEnd(object sender, EventArgs e)
        {
            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.EndGame));
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
            CurrentCharacter = Game.CurrentCharacter;
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
            get => _currentCharacter;
            set
            {
                if (Set(ref _currentCharacter, value))
                {
                    Actions = _currentCharacter.Actions.Select(a =>
                    new Tuple<RelayCommand, IGameAction>(new RelayCommand(
                        () => DoAction(a.Key),
                        () => CanExecuteAction(a.Key), true), _currentCharacter.Actions[a.Key])).ToList();
                }
            }
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

        public bool IsPlayerHandVisible
        {
            get { return _isPlayerHandVisible; }
            set { Set(ref _isPlayerHandVisible, value); }
        }

        public RelayCommand MoveActionCommand { get; set; }

        public RelayCommand PlayerDiscardPileCommand { get; set; }

        public RelayCommand ShareActionCommand { get; set; }

        public RelayCommand TreatActionCommand { get; set; }

        private void Game_CardsSelecting(object sender, CardsSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);

            IsPlayerHandVisible = false;
            ActionViewModel = new CardsSelectionViewModel(e.Cards, (Card c) =>
            {
                //HideActionViews();
                e.SelectionDelegate(c);
            });
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

            foreach (var city in Game.WorldMap.Cities.Values)
            {
                city.IsSelectable = false;
            }
        }

        private void DoAction(string actionString)
        {
            Game.DoAction(actionString);
        }

        private void Game_ActionDone(object sender, EventArgs e)
        {
            IsPlayerHandVisible = true;
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
            foreach (var action in Actions)
            {
                action.Item1.RaiseCanExecuteChanged();
            }
            EventsCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(InfoText));
        }

        private bool CanExecuteAction(string key)
        {
            if (key != null && CurrentCharacter.Actions.TryGetValue(key, out IGameAction gameAction))
            {
                return gameAction.CanExecute(Game);
            }
            else
            {
                return false;
            }
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
    }
}
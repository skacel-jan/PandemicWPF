using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Pandemic.Cards;
using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private IList<Tuple<RelayCommand, IGameAction>> _actions;
        private ViewModelBase _actionViewModel;
        private Character _currentCharacter;
        private ViewModelBase _infoViewModel;
        private bool _isActionVisible;
        private bool _isInfoVisible;
        private bool _isPlayerHandVisible = true;

        public BoardViewModel(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            CurrentCharacter = Game.CurrentCharacter;

            Game.InfoChanged += Game_InfoChanged;
            Game.ActionDone += Game_ActionDone;
            Game.CharacterChanged += Game_CharacterChanged;
            Game.DiseaseSelecting += Game_DiseaseSelecting;
            Game.ShareTypeSelecting += Game_ShareTypeSelecting;
            Game.MoveTypeSelecting += Game_MoveTypeSelecting;
            Game.CharacterSelecting += Game_CharacterSelecting;
            Game.CardSelecting += Game_CardsSelecting;
            Game.GameEnd += Game_GameEnd;
            Game.GamePhaseChanged += Game_GamePhaseChanged;
            Game.EventFinished += Game_EventFinished;

            CancelCommand = new RelayCommand(Cancel, () => IsActionPhase);

            EventsCommand = new RelayCommand(SelectEvent, () => Game.EventCards.Count > 0);

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);
            InfectionDiscardPileCommand = new RelayCommand(ShowInfectionDiscardPile);
        }

        public IList<Tuple<RelayCommand, IGameAction>> Actions { get => _actions; set => Set(ref _actions, value); }

        public ViewModelBase ActionViewModel
        {
            get { return _actionViewModel; }
            set
            {
                Set(ref _actionViewModel, value);
                IsActionVisible = value == null ? false : true;
            }
        }

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
                        () => IsActionPhase && CanExecuteAction(a.Key), true), _currentCharacter.Actions[a.Key])).ToList();
                }
            }
        }


        public RelayCommand EventsCommand { get; }

        public Game Game { get; }

        public RelayCommand InfectionDiscardPileCommand { get;  }

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

        public bool IsActionPhase => Game.GamePhase is ActionPhase;

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

        public RelayCommand PlayerDiscardPileCommand { get; set; }

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

        private void DoAction(string actionString)
        {
            Game.DoAction(actionString);
        }

        private void Game_ActionDone(object sender, EventArgs e)
        {
            IsPlayerHandVisible = true;
            RefreshAllCommands();
        }

        private void Game_CardsSelecting(object sender, CardsSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);

            IsPlayerHandVisible = false;
            ActionViewModel = new CardsSelectionViewModel(e.Cards, (Card c) =>
            {
                var tempActionVm = ActionViewModel;
                var tempInfoVm = InfoViewModel;

                HideActionViews();
                if (!e.SelectionDelegate(c))
                {
                    ActionViewModel = tempActionVm;
                    InfoViewModel = tempInfoVm;
                }
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

        private void Game_EventFinished(object sender, EventArgs e)
        {
            RefreshAllCommands();
        }

        private void Game_GameEnd(object sender, EventArgs e)
        {
            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.EndGame));
        }

        private void Game_GamePhaseChanged(object sender, GamePhaseChangedEventArgs e)
        {
            RefreshAllCommands();
        }

        private void Game_CharacterChanged(object sender, EventArgs e)
        {
            CurrentCharacter = Game.CurrentCharacter;
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

        private void Game_MoveTypeSelecting(object sender, MoveTypeSelectingEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.Text);
            ActionViewModel = new MoveSelectionViewModel(e.Moves, (IMoveAction type) =>
            {
                HideActionViews();
                e.SelectionDelegate(type);
            });
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

        private void HideActionViews()
        {
            InfoViewModel = null;
            ActionViewModel = null;
        }

        private void RefreshAllCommands()
        {
            foreach (var action in Actions)
            {
                action.Item1.RaiseCanExecuteChanged();
            }
            EventsCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(InfoText));
        }

        private void SelectEvent()
        {
            if (ActionViewModel is EventsViewModel)
            {
                ActionViewModel = null;
            }
            else
            {
                var eventsViewModel = new EventsViewModel(Game.EventCards, Game);
                eventsViewModel.EventSelected += (sender, e) =>
                {
                    ActionViewModel = null;
                    e.EventCard.PlayEvent(Game);
                };
                ActionViewModel = eventsViewModel;
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
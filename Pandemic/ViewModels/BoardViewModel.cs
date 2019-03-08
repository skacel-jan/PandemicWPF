using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using Pandemic.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        private readonly Stack<ViewModelBase> _actionViewModels;

        public WorldMapViewModel WorldMapViewModel { get; }

        public BoardViewModel(GameFactory gameFactory, IDialogService dialogService)
        {
            Game = gameFactory?.CreateGame() ?? throw new ArgumentNullException(nameof(gameFactory));
            DialogService = dialogService;
            Game.SelectionService.Selecting += SelectingAction;
            Game.SelectionService.SelectionFinished += ActionFinished;

            CurrentCharacter = Game.CurrentCharacter;

            Game.ActionDone += Game_ActionDone;
            Game.Characters.PropertyChanged += Characters_PropertyChanged;
            Game.PropertyChanged += Game_PropertyChanged;
            Game.DiseaseSelecting += Game_DiseaseSelecting;
            Game.ShareTypeSelecting += Game_ShareTypeSelecting;
            Game.MoveTypeSelecting += Game_MoveTypeSelecting;
            Game.GameEnded += Game_GameEnd;
            Game.GamePhaseChanged += Game_GamePhaseChanged;

            foreach (var eventCard in Game.PlayerDeck.EventCards)
            {
                eventCard.EventFinished += Game_EventFinished;
            }

            CancelCommand = new RelayCommand(Cancel, () => IsActionPhase);

            EventsCommand = new RelayCommand(SelectEvent, () => Game.EventCards.Count() > 0);

            GameMenuCommand = new RelayCommand(() => DialogService.ShowDialog("Game menu", new GameMenuViewModel(Game)));

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);
            InfectionDiscardPileCommand = new RelayCommand(ShowInfectionDiscardPile);

            _actionViewModels = new Stack<ViewModelBase>();

            WorldMapViewModel = new WorldMapViewModel(Game.WorldMap);
        }

        private void Game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.Info))
            {
                InfoChanged();
            }
        }

        private void SelectingAction(object sender, ViewModelEventArgs e)
        {
            ActionViewModel = e.ViewModel;
        }

        private void ActionFinished(object sender, EventArgs e)
        {
            ActionViewModel = null;
        }

        private void Characters_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CircularCollection<Character>.Current))
            {
                CurrentCharacter = Game.CurrentCharacter;
            }
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

        public RelayCommand GameMenuCommand { get; }

        public Character CurrentCharacter
        {
            get => _currentCharacter;
            set
            {
                if (Set(ref _currentCharacter, value))
                {
                    SetNextCharacter();
                }
            }
        }

        public void SetNextCharacter()
        {
            Actions = _currentCharacter.Actions.Select(a =>
                    new Tuple<RelayCommand, IGameAction>(new RelayCommand(
                        () => DoAction(a.Value),
                        () => IsActionPhase && CanExecuteAction(a.Value), true), _currentCharacter.Actions[a.Key])).ToList();
        }

        public RelayCommand EventsCommand { get; }

        public Game Game { get; }
        public IDialogService DialogService { get; }
        public RelayCommand InfectionDiscardPileCommand { get; }

        public string InfoText
        {
            get { return $"{CurrentCharacter.Role}'s turn, {Game.Actions} actions left"; }
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

            foreach (var city in Game.WorldMap.Cities)
            {
                city.IsSelectable = false;
            }
        }

        private bool CanExecuteAction(IGameAction gameAction)
        {
            return gameAction.CanExecute(Game);
        }

        private void DoAction(IGameAction action)
        {
            Game.DoAction(action);
        }

        private void Game_ActionDone(object sender, EventArgs e)
        {
            IsPlayerHandVisible = true;
            RefreshAllCommands();
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
            MessengerInstance.Send(new NavigateToViewModelMessage(MessageTokens.MainMenu));
        }

        private void Game_GamePhaseChanged(object sender, GamePhaseChangedEventArgs e)
        {
            RefreshAllCommands();
        }

        private void InfoChanged()
        {
            if (Game.Info == null)
            {
                InfoViewModel = null;
            }
            else
            {
                if (Game.Info.Action == null)
                {
                    InfoViewModel = new TextViewModel(Game.Info.Text);
                }
                else
                {
                    InfoViewModel = new TextViewModel(Game.Info.Text, Game.Info.Action, Game.Info.ButtonText);
                }
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
                ActionViewModel = _actionViewModels.Count > 0 ? _actionViewModels.Pop() : null;
            }
            else
            {
                if (ActionViewModel != null)
                {
                    _actionViewModels.Push(ActionViewModel);
                }

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
            if (InfoViewModel != null && InfoViewModel is InfectionCardsViewModel)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new InfectionCardsViewModel(Game.Infection.DiscardPile.Cards);
            }
        }

        private void ShowPlayerDiscardPile()
        {
            if (InfoViewModel != null && InfoViewModel is PlayerCardsViewModel)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new PlayerCardsViewModel(Game.PlayerDiscardPile.Cards);
            }
        }



    }
}
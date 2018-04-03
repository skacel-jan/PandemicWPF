﻿using GalaSoft.MvvmLight;
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
        private bool _isInfoVisible;
        private string _actionEvent;

        public BoardViewModel(Board board, TurnStateMachine turnStateMachine, ActionStateMachine actionStateMachine)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));

            ActionStateMachine = actionStateMachine;
            ActionStateMachine.CitySelecting += ActionStateMachine_CitySelecting;
            ActionStateMachine.CardsSelecting += ActionStateMachine_CardsSelecting;
            ActionStateMachine.DiseaseSelecting += ActionStateMachine_DiseaseSelecting;
            ActionStateMachine.ShareTypeSelecting += ActionStateMachine_ShareTypeSelecting;
            ActionStateMachine.CharacterSelecting += ActionStateMachine_CharacterSelecting;
            ActionStateMachine.MoveTypeSelecting += ActionStateMachine_MoveTypeSelecting;
            ActionStateMachine.ActionDone += ActionStateMachine_ActionDone;

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
            ShareActionCommand = new RelayCommand(() => DoAction(ActionTypes.Share), () => CurrentCharacter.CanShareKnowledge());
            BuildActionCommand = new RelayCommand(() => DoAction(ActionTypes.Build), () => CurrentCharacter.CanBuildResearchStation());
            DiscoverCureActionCommand = new RelayCommand(() => DoAction(ActionTypes.Discover), () => CurrentCharacter.CanDiscoverCure());

            CancelCommand = new RelayCommand(Cancel);

            PlayerDiscardPileCommand = new RelayCommand(ShowPlayerDiscardPile);
            InfectionDiscardPileCommand = new RelayCommand(ShowInfectionDiscardPile);

            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.CitySelected, CitySelected);
            MessengerInstance.Register<GenericMessage<MapCity>>(this, MessageTokens.InstantMove, InstantMove);

            ActionStateMachine.Start();
            TurnStateMachine.Start();
        }

        private void ShowInfectionDiscardPile()
        {
            if (InfoViewModel != null)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardsSelectionViewModel(Board.InfectionDiscardPile.Cards, null);
            }
        }

        private void ShowPlayerDiscardPile()
        {
            if (InfoViewModel != null)
            {
                InfoViewModel = null;
            }
            else
            {
                InfoViewModel = new CardsSelectionViewModel(Board.PlayerDiscardPile.Cards, null);
            }
        }

        private void ActionStateMachine_MoveTypeSelecting(object sender, MoveTypeEventArgs e)
        {
            ActionViewModel = new MoveSelectionViewModel(e.Moves, e.SelectionDelegate);
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

        public RelayCommand PlayerDiscardPileCommand { get; set; }
        public RelayCommand InfectionDiscardPileCommand { get; private set; }
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

        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { Set(ref _isInfoVisible, value); }
        }

        public RelayCommand MoveActionCommand { get; set; }

        public MapCity SelectedCity { get; private set; }

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

            ActionViewModel = new CardsSelectionViewModel(e.Cards, e.SelectionDelegate);
        }

        private void ActionStateMachine_CitySelecting(object sender, InfoTextEventArgs e)
        {
            InfoViewModel = new TextViewModel(e.InfoText);
        }

        private void ActionStateMachine_DiseaseSelecting(object sender, EventArgs e)
        {
            ActionViewModel = new DiseaseSelectionViewModel(new List<DiseaseColor>(CurrentCharacter.CurrentMapCity.DiseasesToTreat),
                (disease) => ActionStateMachine.DoAction(_actionEvent, disease));
        }

        private void ActionStateMachine_CharacterSelecting(object sender, EventArgs e)
        {
            ActionViewModel = new CharacterSelectionViewModel(TurnStateMachine.Characters.Where(x => x != CurrentCharacter));
        }

        private void ActionStateMachine_ShareTypeSelecting(object sender, ShareTypeEventArgs e)
        {
            ActionViewModel = new ShareTypeSelectionViewModel(e.SelectionDelegate);
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
                foreach (var city in Board.WorldMap.Cities.Values)
                {
                    city.IsMoveEnabled = false;
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
            ActionStateMachine.DoAction(ActionTypes.DriveOrFerry, mapCityMessage.Content);
        }

        private void DoAction(string actionString)
        {
            if (TurnStateMachine.Actions > 0)
            {
                _actionEvent = actionString;
                ActionStateMachine.DoAction(_actionEvent);
            }
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
                        foreach (var playerCard in selectedCards.Cast<PlayerCard>())
                        {
                            CurrentCharacter.RemoveCard(playerCard);
                            Board.PlayerDiscardPile.AddCard(playerCard);
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
            ActionStateMachine.CurrentCharacter = TurnStateMachine.CurrentCharacter;
            InfoViewModel = null;
            ActionViewModel = null;
            RaisePropertyChanged(nameof(InfoText));
            RaisePropertyChanged(nameof(CurrentCharacter));
            RefreshAllCommands();
        }
    }
}
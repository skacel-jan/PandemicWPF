using Appccelerate.StateMachine;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using static Pandemic.TurnStateMachine;

namespace Pandemic
{
    public class TurnStateMachine : ObservableObject
    {
        private int _actions;
        private Character _currentCharacter;
        private PassiveStateMachine<TurnStates, TurnEvents> _passiveStateMachine;

        public TurnStateMachine(Queue<Character> _characters)
        {
            Characters = _characters;

            _passiveStateMachine = new PassiveStateMachine<TurnStates, TurnEvents>();

            _passiveStateMachine.In(TurnStates.StartOfTurn)
                .ExecuteOnEntry(ExecuteStartTurnEvent)
                .On(TurnEvents.Next).Goto(TurnStates.ActionPhase);

            _passiveStateMachine.In(TurnStates.ActionPhase)
                .ExecuteOnExit(() => OnActionPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next)
                    .If(() => Actions == 0).Goto(TurnStates.DrawingPhase)
                    .Otherwise().Execute(ExecuteActionEvent);

            _passiveStateMachine.In(TurnStates.DrawingPhase)                
                .On(TurnEvents.Next)
                    .If(() => Draws == 0).Goto(TurnStates.InfectionPhase)
                        .Execute(() => OnDrawingPhaseEnded(EventArgs.Empty))
                    .Otherwise().Execute(ExecuteDrawEvent)
                .On(TurnEvents.GameOver).Goto(TurnStates.GameLost);

            _passiveStateMachine.In(TurnStates.InfectionPhase)
                .ExecuteOnExit(() => OnInfectionPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next)
                    .If(() => Infections == 0).Goto(TurnStates.EndOfTurn)
                    .Otherwise().Execute(ExecuteInfectionEvent);

            _passiveStateMachine.In(TurnStates.EndOfTurn)
                .On(TurnEvents.Next).Goto(TurnStates.StartOfTurn);

            _passiveStateMachine.In(TurnStates.GameLost)                
                .ExecuteOnEntry(() => GameLost?.Invoke(this, new GenericEventArgs<string>("Game over: No more cards")));
        }

        public event EventHandler ActionDone;

        public event EventHandler ActionPhaseEnded;

        public event EventHandler Discarding;

        public event EventHandler<GenericEventArgs<int>> DoInfection;

        public event EventHandler DrawDone;

        public event EventHandler DrawingPhaseEnded;

        public event EventHandler InfectionPhaseEnded;

        public event EventHandler TurnStarted;

        public event EventHandler<GenericEventArgs<string>> GameLost;

        public enum TurnEvents
        {
            Next,
            GameOver
        }

        public enum TurnStates
        {
            StartOfTurn,
            ActionPhase,
            DrawingPhase,
            InfectionPhase,
            EndOfTurn,
            GameWon,
            GameLost
        }

        public int Actions
        {
            get => _actions;
            set => Set(ref _actions, value);
        }

        public Character CurrentCharacter
        {
            get => _currentCharacter;
            private set => Set(ref _currentCharacter, value);
        }

        public int Draws { get; private set; }

        public Queue<Character> Characters { get; set; }

        public int Infections { get; private set; }
        public int InfectionRate { get; set; }

        public void DoAction()
        {
            _passiveStateMachine.Fire(TurnEvents.Next);
        }

        public void Start()
        {
            _passiveStateMachine.Initialize(TurnStates.StartOfTurn);
            _passiveStateMachine.Start();
        }

        public void Stop()
        {
            _passiveStateMachine.Stop();
        }

        protected void OnActionDone(EventArgs e)
        {
            ActionDone?.Invoke(this, e);
        }

        protected void OnActionPhaseEnded(EventArgs e)
        {
            ActionPhaseEnded?.Invoke(this, e);
        }

        protected void OnDrawDone(EventArgs e)
        {
            DrawDone?.Invoke(this, e);
        }

        protected void OnDrawingPhaseEnded(EventArgs e)
        {
            DrawingPhaseEnded?.Invoke(this, e);
        }

        protected void OnInfectionDone(GenericEventArgs<int> e)
        {
            DoInfection?.Invoke(this, e);
        }

        protected void OnInfectionPhaseEnded(EventArgs e)
        {
            InfectionPhaseEnded?.Invoke(this, e);
        }

        protected void OnTurnStarted(EventArgs e)
        {
            TurnStarted?.Invoke(this, e);
        }

        private void ExecuteActionEvent()
        {
            Actions--;
            OnActionDone(new GenericEventArgs<int>(Actions));
            if (Actions == 0)
            {
                _passiveStateMachine.Fire(TurnEvents.Next);
            }
        }

        private void ExecuteDiscardEvent()
        {
            Discarding?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteDrawEvent()
        {
            OnDrawDone(new GenericEventArgs<int>(Draws));
            Draws -= 1;
            _passiveStateMachine.Fire(TurnEvents.Next);
        }

        private void ExecuteInfectionEvent()
        {
            Infections--;
            OnInfectionDone(new GenericEventArgs<int>(Infections));
            if (Infections == 0)
            {
                _passiveStateMachine.Fire(TurnEvents.Next);
            }
        }

        private void ExecuteStartTurnEvent()
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.IsActive = false;
                CurrentCharacter.CurrentMapCity.CharactersChanged();
            }

            CurrentCharacter = Characters.Dequeue();
            Actions = CurrentCharacter.ActionsCount;
            Characters.Enqueue(CurrentCharacter);

            CurrentCharacter.IsActive = true;
            CurrentCharacter.CurrentMapCity.CharactersChanged();

            Draws = 2;
            Infections = 2;
            OnTurnStarted(EventArgs.Empty);
            _passiveStateMachine.Fire(TurnEvents.Next);
        }

        public void GameOver()
        {
            _passiveStateMachine.Fire(TurnEvents.GameOver);
        }
    }
}
using Appccelerate.StateMachine;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class TurnStateMachine : ObservableObject
    {
        private int _actions;
        private Character _currentCharacter;
        private PassiveStateMachine<TurnStates, TurnEvents> _passiveStateMachine;

        private int turns;

        public TurnStateMachine(Queue<Character> _characters)
        {
            Characters = _characters;
            ExecuteStartTurnEvent();

            _passiveStateMachine = new PassiveStateMachine<TurnStates, TurnEvents>();
            _passiveStateMachine.In(TurnStates.ActionPhase)
                .On(TurnEvents.Next)
                    .If(() => Actions > 1).Execute(ExecuteActionEvent)
                    .Otherwise().Goto(TurnStates.DrawingPhase).Execute(ExecuteActionEvent);

            _passiveStateMachine.In(TurnStates.DrawingPhase)
                .ExecuteOnEntry(ExecuteDrawEvent)
                .On(TurnEvents.Next)
                    .If(() => Draws > 0).Execute(ExecuteDrawEvent)
                    .If(() => Draws == 0).Goto(TurnStates.InfectionPhase);

            _passiveStateMachine.In(TurnStates.InfectionPhase)
                .ExecuteOnEntry(ExecuteInfectionEvent)
                .On(TurnEvents.Next)
                    .If(() => Infections > 0).Execute(ExecuteInfectionEvent)
                    .If(() => Infections == 0).Goto(TurnStates.TurnEnd);

            _passiveStateMachine.In(TurnStates.TurnEnd)
                .ExecuteOnEntry(ExecuteStartTurnEvent)
                .On(TurnEvents.Next).Goto(TurnStates.ActionPhase);
        }

        public event EventHandler<GenericEventArgs<int>> ActionDone;

        public event EventHandler<GenericEventArgs<int>> DrawDone;

        public event EventHandler<GenericEventArgs<int>> InfectionDone;

        public enum TurnEvents
        {
            Next
        }

        public enum TurnStates
        {
            ActionPhase,
            DrawingPhase,
            InfectionPhase,
            TurnEnd
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
        public int Infections { get; set; }

        public void DoAction()
        {
            _passiveStateMachine.Fire(TurnEvents.Next);
        }

        public void Start()
        {
            _passiveStateMachine.Initialize(TurnStates.ActionPhase);
            _passiveStateMachine.Start();
        }

        public void Stop()
        {
            _passiveStateMachine.Stop();
        }

        protected void OnActionDone(GenericEventArgs<int> e)
        {
            ActionDone?.Invoke(this, e);
        }

        protected void OnDrawDone(GenericEventArgs<int> e)
        {
            DrawDone?.Invoke(this, e);
        }

        protected void OnInfectionDone(GenericEventArgs<int> e)
        {
            InfectionDone?.Invoke(this, e);
        }

        private void ExecuteActionEvent()
        {
            Actions--;
            OnActionDone(new GenericEventArgs<int>(Actions));
        }

        private void ExecuteDrawEvent()
        {
            Draws--;
            OnDrawDone(new GenericEventArgs<int>(Draws));
        }

        private void ExecuteInfectionEvent()
        {
            Infections--;
            OnInfectionDone(new GenericEventArgs<int>(Infections));
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
            turns++;
        }
    }
}
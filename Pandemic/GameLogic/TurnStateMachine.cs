﻿using Appccelerate.StateMachine;
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
                .ExecuteOnExit(() => OnActionPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next)
                    .If(() => Actions == 0).Goto(TurnStates.DrawingPhase)
                    .Otherwise().Execute(ExecuteActionEvent);

            _passiveStateMachine.In(TurnStates.DrawingPhase)
                .ExecuteOnExit(() => OnDrawingPhaseEnded(EventArgs.Empty))
                .On(TurnEvents.Next).Goto(TurnStates.InfectionPhase).Execute(ExecuteDrawEvent);

            _passiveStateMachine.In(TurnStates.InfectionPhase)
                .On(TurnEvents.Next)
                    .If(() => Infections == 0).Goto(TurnStates.TurnEnd)
                    .Otherwise().Execute(ExecuteInfectionEvent);

            _passiveStateMachine.In(TurnStates.TurnEnd)
                .ExecuteOnEntry(() => OnInfectionPhaseEnded(EventArgs.Empty))
                .ExecuteOnExit(ExecuteStartTurnEvent)
                .On(TurnEvents.Next).Goto(TurnStates.ActionPhase);
        }

        public event EventHandler ActionDone;

        public event EventHandler ActionPhaseEnded;

        public event EventHandler DrawDone;

        public event EventHandler DrawingPhaseEnded;

        public event EventHandler<GenericEventArgs<int>> InfectionDone;

        public event EventHandler InfectionPhaseEnded;

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
            InfectionDone?.Invoke(this, e);
        }

        protected void OnInfectionPhaseEnded(EventArgs e)
        {
            InfectionPhaseEnded?.Invoke(this, e);
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

        private void ExecuteDrawEvent()
        {
            OnDrawDone(new GenericEventArgs<int>(Draws));
            Draws -= 2;
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
            turns++;
        }
    }
}
using System;

namespace Pandemic.GameLogic.Actions
{
    public abstract class EventAction : IGameAction
    {
        public string Name => ActionTypes.Event;

        protected Action ActionCallback { get; set; }
        protected Game Game { get; set; }

        public bool CanExecute(Game game) => true;

        public void Execute(Game game, Action callbackAction)
        {
            ActionCallback = callbackAction;
            Game = game;

            Execute();
        }

        protected abstract void Execute();

        protected virtual void FinishAction()
        {
            ActionCallback?.Invoke();
            ActionCallback = null;
            Game = null;
        }
    }
}
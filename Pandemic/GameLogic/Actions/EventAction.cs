using System;

namespace Pandemic.GameLogic.Actions
{
    public abstract class EventAction : IGameAction
    {
        protected Action _actionCallback;

        protected Game _game;

        public bool CanExecute(Game game) => true;

        public void Execute(Game game, Action callbackAction)
        {
            _actionCallback = callbackAction;
            _game = game;

            Execute();
        }

        protected abstract void Execute();

        protected virtual void FinishAction()
        {
            _actionCallback?.Invoke();
            _actionCallback = null;
            _game = null;
        }
    }
}
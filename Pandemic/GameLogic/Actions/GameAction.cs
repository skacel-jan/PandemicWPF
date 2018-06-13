using System;

namespace Pandemic.GameLogic.Actions
{
    public interface IGameAction
    {
        bool CanExecute(Game game);
        void Execute(Game game, Action callbackAction);
    }

    public abstract class GameAction : IGameAction
    {
        protected Game _game;
        protected Action _actionCallback;

        public Character Character { get; protected set; }

        public GameAction(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public void Execute(Game game, Action callbackAction)
        {
            _actionCallback = callbackAction;
            _game = game;

            Execute();
        }

        protected abstract void Execute();
        public abstract bool CanExecute(Game game);

        protected virtual void FinishAction()
        {
            _actionCallback?.Invoke();
            _actionCallback = null;
            _game = null;
        }
    }
}

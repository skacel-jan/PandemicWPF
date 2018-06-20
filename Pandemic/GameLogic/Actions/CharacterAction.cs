using System;

namespace Pandemic.GameLogic.Actions
{
    public interface IGameAction
    {
        bool CanExecute(Game game);
        void Execute(Game game, Action callbackAction);
    }

    public abstract class CharacterAction : IGameAction
    {
        protected Game _game;
        protected Action _actionFinishedCallback;

        public Character Character { get; protected set; }

        public CharacterAction(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public void Execute(Game game, Action callbackAction)
        {
            _actionFinishedCallback = callbackAction;
            _game = game;

            Execute();
        }

        protected abstract void Execute();
        public abstract bool CanExecute(Game game);

        protected virtual void FinishAction()
        {
            _actionFinishedCallback?.Invoke();
            _actionFinishedCallback = null;
            _game = null;
        }
    }
}

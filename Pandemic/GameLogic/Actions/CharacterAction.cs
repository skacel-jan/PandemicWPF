using System;

namespace Pandemic.GameLogic.Actions
{
    public interface IGameAction
    {
        string Name { get; }

        bool CanExecute(Game game);

        void Execute(Game game, Action callbackAction);
    }

    public abstract class CharacterAction : IGameAction
    {
        protected CharacterAction(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public Character Character { get; protected set; }
        public abstract string Name { get; }
        protected Action ActionFinishedCallback { get; set; }
        protected Game Game { get; set; }

        public abstract bool CanExecute(Game game);

        public void Execute(Game game, Action callbackAction)
        {
            ActionFinishedCallback = callbackAction;
            Game = game;

            Execute();
        }

        protected abstract void Execute();

        protected virtual void FinishAction()
        {
            ActionFinishedCallback?.Invoke();
            ActionFinishedCallback = null;
            Game = null;
        }
    }
}
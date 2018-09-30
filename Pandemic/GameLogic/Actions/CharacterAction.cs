using System;

namespace Pandemic.GameLogic.Actions
{

    public abstract class CharacterAction : IGameAction
    {
        protected CharacterAction(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public Character Character { get; set; }
        public abstract string Name { get; }
        protected Action ActionFinishedCallback { get; set; }
        public Game Game { get; set; }

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
            Game.Info = null;
            ActionFinishedCallback();
        }

        internal void Next()
        {
            throw new NotImplementedException();
        }
    }
}
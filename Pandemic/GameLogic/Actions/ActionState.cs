using System;

namespace Pandemic.GameLogic.Actions
{
    public class ActionState
    {
        public Predicate<Game> Predicate { get; }
        public Action<Game> Action { get; }
        public Game Game { get; }

        public ActionState(Predicate<Game> predicate, Action<Game> action, Game game)
        {
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Execute()
        {
            Action(Game);
        }
    }
}
using Pandemic.GameLogic.Actions;
using System;

namespace Pandemic.GameLogic
{
    public class GameOverPhase : IGamePhase
    {
        public Game Game { get; }

        public GameOverPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Action(IGameAction action)
        {
        }

        public void End()
        {
        }

        public void Start()
        {
            Game.Info = new GameInfo("Game over", "Main Menu", () => { Game.EndGame(); });
        }
    }
}
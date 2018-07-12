using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class GameOverPhase : IGamePhase
    {
        public Game Game { get; }

        public GameOverPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Action(string actionType)
        {
        }

        public void End()
        {
        }

        public void Start()
        {
            Game.Info = new GameInfo("Game over", "Main Menu", () => { Game.EndGame() ; });
        }
    }
}

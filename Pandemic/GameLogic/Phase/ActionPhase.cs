using Pandemic.GameLogic.Actions;
using System;
using System.Linq;

namespace Pandemic.GameLogic
{
    public class ActionPhase : IGamePhase
    {
        public ActionPhase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));

            game.WorldMap.CityDoubleClicked += WorldMap_CityDoubleClicked;
        }

        private void WorldMap_CityDoubleClicked(object sender, EventArgs e)
        {
            var city = sender as MapCity;
            if (Game.CurrentCharacter.CanMoveToCity(Game, city))
            {
                Game.CurrentCharacter.CurrentMapCity = city;
                Game.Actions--;
                Continue();
            }
        }

        public Game Game { get; }

        public void Continue()
        {
            // Check if any character has more cards in hand then is his hand limit
            // After each action there can be only one
            var character = Game.Characters.FirstOrDefault(c => c.HasMoreCardsThenLimit);
            if (character != null)
            {
                Game.ResolveAction(new DiscardPlayerCardAction(character, Game));
                return;
            }

            if (Game.Actions == 0)
            {
                Game.WorldMap.CityDoubleClicked -= WorldMap_CityDoubleClicked;
                Game.ChangeGamePhase(new DrawPhase(Game));
            }
        }

        public void End()
        {
            Game.CurrentCharacter.IsActive = false;
            Game.Info = new GameInfo($"Action phase has ended.", "Continue to drawing phase", () => Game.Continue());
        }

        public void Start()
        {
            Game.Turn++;
            Game.CurrentCharacter.IsActive = true;
            Game.Actions = Game.CurrentCharacter.ActionsCount;
            Game.Info = null;
        }
    }
}
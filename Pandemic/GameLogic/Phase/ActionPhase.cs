using Pandemic.GameLogic.Actions;
using System;

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
                FinishAction();
            }
        }

        public Game Game { get; }

        public void Action(IGameAction action)
        {
            action.Execute(Game, FinishAction);
            //var build = new Build(Game.CurrentCharacter);
            //build.Execute(this);
        }

        public void End()
        {
            Game.CurrentCharacter.IsActive = false;
            Game.Info = new GameInfo($"Action phase has ended.", "Continue to drawing phase", () => Game.DoAction(null));
        }

        public void Start()
        {
            Game.Turn++;
            Game.Characters.Current.IsActive = true;
            Game.Actions = Game.Characters.Current.ActionsCount;
            Game.Info = null;
        }

        public void FinishAction()
        {
            Game.Actions--;

            if (Game.Actions == 0)
            {
                Game.WorldMap.CityDoubleClicked -= WorldMap_CityDoubleClicked;
                Game.ChangeGamePhase(new DrawPhase(Game));
            }
        }
    }
}
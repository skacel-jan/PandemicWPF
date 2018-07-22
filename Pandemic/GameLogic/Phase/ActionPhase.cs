using Pandemic.Cards;
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
                Game.MoveCharacter(Game.CurrentCharacter, city);
                FinishAction();
            }            
        }

        public Game Game { get; }

        public void Action(string actionType)
        {
            if (actionType == ActionTypes.Event)
            {
                SelectEventCard(Game);
            }
            else if (Game.CurrentCharacter.Actions.TryGetValue(actionType, out IGameAction gameAction))
            {
                gameAction.Execute(Game, FinishAction);
            }
        }

        public void End()
        {
            Game.CurrentCharacter.IsActive = false;
            Game.Info = new GameInfo($"Action phase has ended.", "Continue to drawing phase", () => Game.DoAction("Next"));
        }

        public void Start()
        {
            Game.Turn++;
            Game.Characters.Current.IsActive = true;
            Game.Actions = Game.Characters.Current.ActionsCount;
            Game.Info = null;
        }

        private void FinishAction()
        {
            Game.Actions--;

            if (Game.Actions == 0)
            {
                Game.WorldMap.CityDoubleClicked -= WorldMap_CityDoubleClicked;
                Game.ChangeGamePhase(new DrawPhase(Game));
            }
        }

        private void SelectEventCard(Game game)
        {
            var callback = new Func<Card, bool>(c =>
            {
                var eventCard = c as EventCard;

                eventCard.PlayEvent(game);
                return true;
            });

            game.SelectCard(game.EventCards, callback, "Select event card");
        }
    }
}
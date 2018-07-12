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

            game.WorldMap.MovedToCity += WorldMap_MovedToCity;            
        }

        private void WorldMap_MovedToCity(object sender, EventArgs e)
        {
            Game.MoveCharacter(Game.CurrentCharacter, sender as MapCity);
            FinishAction();
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
            Game.Characters.Current.IsActive = true;
            Game.Actions = Game.Characters.Current.ActionsCount;
            Game.Info = null;
        }

        private void FinishAction()
        {
            Game.Actions--;
            Game.Info = null;

            if (Game.Actions == 0)
            {
                Game.WorldMap.MovedToCity -= WorldMap_MovedToCity;
                Game.GamePhase = new DrawPhase(Game);
            }
        }

        private void SelectEventCard(Game game)
        {
            var action = new Action<Card>(c =>
            {
                var eventCard = c as EventCard;

                eventCard.PlayEvent(game);
            });

            game.SelectCard(game.EventCards, action, "Select event card");
        }
    }
}
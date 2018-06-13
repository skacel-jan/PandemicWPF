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
                gameAction.Execute(Game, ActionFinished);
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
            Game.CurrentCharacter = Game.Characters.Current;
            Game.Actions = Game.Characters.Current.ActionsCount;
            Game.Info = null;
        }

        private void ActionFinished()
        {
            Game.Actions--;

            if (Game.Actions == 0)
            {
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
using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class ContingencyPlannerAction : CharacterAction
    {
        public ContingencyPlannerAction(Character character) : base(character)
        {
        }

        public override string Name => "Special";

        public override bool CanExecute(Game game)
        {
            return game.PlayerDiscardPile.Cards.Any(c => c is EventCard);
        }

        protected override void Execute()
        {
            Game.SelectCard(Game.PlayerDiscardPile.Cards.OfType<EventCard>(), CardSelectedCallback, "Select event card");
        }

        private bool CardSelectedCallback(Card card)
        {
            if (card is EventCard eventCard)
            {
                Game.PlayerDiscardPile.RemoveCard(eventCard);
                Game.EventCards.Add(eventCard);
                eventCard.EventFinished += EventCard_EventFinished;
                FinishAction();
                return true;
            }

            return false;
        }

        private void EventCard_EventFinished(object sender, EventArgs e)
        {
            var eventCard = sender as EventCard;
            Game.PlayerDiscardPile.RemoveCard(eventCard);
            Game.RemovedCards.AddCard(eventCard);

            eventCard.EventFinished -= EventCard_EventFinished;
        }
    }
}

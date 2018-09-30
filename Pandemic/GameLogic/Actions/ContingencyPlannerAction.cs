using Pandemic.Cards;
using System;
using System.Linq;

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
            Game.SelectionService.Select(new SelectAction<Card>(SetCard, Game.PlayerDiscardPile.Cards.OfType<EventCard>(),
                "Select event card to save"));            
        }

        private void SetCard(Card card)
        {
            EventCard eventCard = (EventCard)card;

            Game.PlayerDiscardPile.RemoveCard(eventCard);
            Game.EventCards.Add(eventCard);
            eventCard.EventFinished += EventCard_EventFinished;
            FinishAction();
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
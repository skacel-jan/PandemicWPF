using Pandemic.Decks;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Cards
{
    public class ResilientPopulationCard : EventCard
    {
        public ResilientPopulationCard(CardSelectionService cardSelectionService, DecksService decksService) : base("Resilient population")
        {
            CardSelectionService = cardSelectionService ?? throw new ArgumentNullException(nameof(cardSelectionService));
            DecksService = decksService ?? throw new ArgumentNullException(nameof(decksService));
        }

        public CardSelectionService CardSelectionService { get; }
        public DecksService DecksService { get; }

        public override void PlayEvent()
        {
            var action = new Action<Card>((Card card) =>
            {
                if (card is InfectionCard infectionCard)
                {
                    DecksService.RemoveCard(infectionCard);
                    Character.RemoveCard(this);
                }

                OnEventFinished(EventArgs.Empty);
            });

            CardSelectionService.SelectCard(DecksService.InfectionDiscardPile.Cards, "Select infection card", action);
        }
    }
}
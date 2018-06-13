using Pandemic.Decks;
using Pandemic.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.Cards
{
    public class ResilientPopulationCard : EventCard
    {
        public ResilientPopulationCard() : base("Resilient population")
        {
        }

        public override void PlayEvent(Game game)
        {
            var action = new Action<Card>((Card card) =>
            {
                if (card is InfectionCard infectionCard)
                {
                    game.InfectionDiscardPile.RemoveCard(infectionCard);
                    game.RemovedCards.AddCard(card);

                    OnEventFinished(EventArgs.Empty, game);
                }
            });

            game.SelectCard(game.InfectionDiscardPile.Cards, action, "Select infection card");
        }
    }
}
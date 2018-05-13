using Pandemic.Cards;
using Pandemic.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public class DecksService
    {
        public event EventHandler SkipInfectionPhase;

        public DecksService(PlayerDeck playerDeck, Deck<InfectionCard> infectionDeck, EventCardFactory eventCardFactory)
        {
            PlayerDeck = playerDeck ?? throw new ArgumentNullException(nameof(playerDeck));
            InfectionDeck = infectionDeck ?? throw new ArgumentNullException(nameof(infectionDeck));
            EventCardFactory = eventCardFactory ?? throw new ArgumentNullException(nameof(eventCardFactory));
            InfectionDiscardPile = new DiscardPile<InfectionCard>();
            PlayerDiscardPile = new DiscardPile<Card>();
            RemovedCards = new DiscardPile<Card>();

            EventCardFactory.DecksService = this;

            var eventCards = eventCardFactory.GetEventCards().ToList();
            PlayerDeck.AddEventCards(eventCards);

            foreach (var eventCard in eventCards)
            {
                eventCard.EventFinished += (s, e) => EventFinished?.Invoke(s, e);                
            }
        }

        public void RaiseSkipInfectionPhase()
        {
            SkipInfectionPhase?.Invoke(this, EventArgs.Empty);
        }

        public PlayerDeck PlayerDeck { get; }
        public Deck<InfectionCard> InfectionDeck { get; }
        public EventCardFactory EventCardFactory { get; }
        public DiscardPile<InfectionCard> InfectionDiscardPile { get; }
        public DiscardPile<Card> PlayerDiscardPile { get; }
        public DiscardPile<Card> RemovedCards { get; }

        public void RemoveCard(InfectionCard card)
        {
            InfectionDiscardPile.RemoveCard(card);
            RemovedCards.AddCard(card);
        }

        public event EventHandler EventFinished;
    }
}

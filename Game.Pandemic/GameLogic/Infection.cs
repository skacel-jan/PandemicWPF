using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Decks;

namespace Game.Pandemic.GameLogic
{
    public class Infection : ObservableObject
    {
        private int _position;
        private int _rate;

        public Infection(Deck<InfectionCard> infectionDeck) : this(infectionDeck, new DiscardPile<InfectionCard>())
        { }

        public Infection(Deck<InfectionCard> deck, DiscardPile<InfectionCard> discardPile)
        {
            Rate = 2;
            Position = 1;

            DiscardPile = discardPile ?? throw new ArgumentNullException(nameof(discardPile));
            Deck = deck ?? throw new ArgumentNullException(nameof(deck));
        }

        public DiscardPile<InfectionCard> DiscardPile { get; }

        public int Actual { get; set; }

        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public Deck<InfectionCard> Deck { get; }

        public int Rate
        {
            get => _rate;
            set => SetProperty(ref _rate, value);
        }

        public void IncreasePosition()
        {
            Position++;
            if (Position == 3 || Position == 5)
            {
                Rate++;
            }
        }

        public void Reset()
        {
            Actual = Rate;
        }

        public void ShuffleDiscardPileToDeck()
        {
            DiscardPile.Shuffle();
            Deck.AddCards(DiscardPile.Cards);
            DiscardPile.Clear();
        }
    }
}
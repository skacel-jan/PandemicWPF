using GalaSoft.MvvmLight;
using Pandemic.Cards;
using Pandemic.Decks;
using System;

namespace Pandemic.GameLogic
{
    public class Infection : ObservableObject
    {
        private int _position;
        private int _rate;

        public Infection(Deck<InfectionCard> infectionDeck)
        {
            Rate = 2;
            Position = 1;

            Deck = infectionDeck ?? throw new ArgumentNullException(nameof(infectionDeck));
            DiscardPile = new DiscardPile<InfectionCard>();
        }

        public DiscardPile<InfectionCard> DiscardPile { get; private set; }

        public int Actual { get; set; }

        public int Position
        {
            get => _position;
            set => Set(ref _position, value);
        }

        public Deck<InfectionCard> Deck { get; }

        public int Rate
        {
            get => _rate;
            set => Set(ref _rate, value);
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
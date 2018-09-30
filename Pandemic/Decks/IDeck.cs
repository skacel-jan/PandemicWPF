using Pandemic.Cards;
using System.Collections.Generic;

namespace Pandemic.Decks
{
    public interface IDeck<T> where T : Card
    {
        IEnumerable<T> Cards { get; }

        void AddCard(T card, DeckSide side);

        void AddCards(IEnumerable<T> cards, DeckSide side);

        T Draw(DeckSide side);
    }

    public enum DeckSide
    {
        Top,
        Bottom
    }
}
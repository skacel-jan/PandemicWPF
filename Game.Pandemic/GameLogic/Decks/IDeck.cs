using System.Collections.Generic;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Decks
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
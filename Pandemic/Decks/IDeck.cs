using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pandemic.Decks
{
    public interface IDeck<T> where T : Card
    {
        ObservableCollection<T> Cards { get; }
        void AddCard(T card);
        void AddCards(IEnumerable<T> cards);
    }

    public interface IDraw<T> where T : Card
    {
        T Draw();
    }

    public interface IShuffle<T> where T : Card
    {
        void Shuffle();
    }
}
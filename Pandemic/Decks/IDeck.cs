using System.Collections.ObjectModel;

namespace Pandemic.Decks
{
    public interface IShuffle<T> where T: Card
    {
        void Shuffle();
    }

    public interface IDraw<T> where T: Card
    {
        T Draw();
    }

    public interface IDeck<T> where T : Card
    {
        ObservableCollection<T> Cards { get; }
    }
}
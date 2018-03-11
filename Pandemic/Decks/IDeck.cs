using System.Collections.ObjectModel;

namespace Pandemic.Decks
{
    public interface IDeck<T> where T : Card
    {
        ObservableCollection<T> Cards { get; }

        T Draw();
        void Shuffle();
    }
}
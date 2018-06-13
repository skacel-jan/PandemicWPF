using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class CardDiscardedEventArgs : EventArgs
    {
        public CardDiscardedEventArgs(Card card)
        {
            Card = card;
        }

        public Card Card { get; }
    }

    public class CardsSelectingEventArgs : EventArgs
    {
        public CardsSelectingEventArgs(IEnumerable<Card> cards, Action<Card> selectionDelegate, string text)
        {
            Cards = cards;
            Text = text;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<Card> Cards { get; }
        public Action<Card> SelectionDelegate { get; }
        public string Text { get; }
    }

    public class DiseaseSelectingEventArgs : EventArgs
    {
        public DiseaseSelectingEventArgs(IEnumerable<DiseaseColor> diseases, Action<DiseaseColor> selectionDelegate, string text)
        {
            Diseases = diseases;
            Text = text;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<DiseaseColor> Diseases { get; }
        public Action<DiseaseColor> SelectionDelegate { get; }
        public string Text { get; }
    }

    public class CitySelectingEventArgs : EventArgs
    {
        public CitySelectingEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }

    public class CharacterSelectingEventArgs : EventArgs
    {
        public CharacterSelectingEventArgs(IEnumerable<Character> characters, Action<Character> selectionDelegate, string text)
        {
            Characters = characters;
            Text = text;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<Character> Characters { get; }
        public Action<Character> SelectionDelegate { get; }
        public string Text { get; }
    }

    public class MoveTypeEventArgs : EventArgs
    {
        public MoveTypeEventArgs(IEnumerable<IMoveCardAction> moves, Action<IMoveCardAction> selectionDelegate)
        {
            Moves = moves;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<IMoveCardAction> Moves { get; }
        public Action<IMoveCardAction> SelectionDelegate { get; }
    }

    public class OutbreakEventArgs : EventArgs
    {
        public OutbreakEventArgs(City city)
        {
            City = city;
        }

        public City City { get; }
    }

    public class ShareTypeSelectingEventArgs : EventArgs
    {
        public ShareTypeSelectingEventArgs(IEnumerable<ShareType> shareTypes, Action<ShareType> selectionDelegate, string text)
        {
            SelectionDelegate = selectionDelegate ?? throw new ArgumentNullException(nameof(selectionDelegate));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            ShareTypes = shareTypes ?? throw new ArgumentNullException(nameof(shareTypes));
        }

        public Action<ShareType> SelectionDelegate { get; }
        public string Text { get; }
        public IEnumerable<ShareType> ShareTypes { get; }
    }

    public enum ShareType
    {
        Give,
        Take
    }
}
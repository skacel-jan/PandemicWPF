using System;
using System.Collections.Generic;

namespace Pandemic
{
    public class CardDiscardedEventArgs : EventArgs
    {
        public CardDiscardedEventArgs(PlayerCard card)
        {
            Card = card;
        }

        public PlayerCard Card { get; }
    }

    public class CardsSelectingEventArgs : EventArgs
    {
        public CardsSelectingEventArgs(IEnumerable<Card> cards, string text, Action<Card> selectionDelegate)
        {
            Cards = cards;
            Text = text;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<Card> Cards { get; }
        public Action<Card> SelectionDelegate { get; }
        public string Text { get; }
    }

    public class CharacterSelectingEventArgs : EventArgs
    {
        public CharacterSelectingEventArgs(IEnumerable<Character> characters, string text, Action<Character> selectionDelegate)
        {
            Characters = characters;
            Text = text;
            SelectionDelegate = selectionDelegate;
        }

        public IEnumerable<Character> Characters { get; }
        public Action<Character> SelectionDelegate { get; }
        public string Text { get; }
    }

    public class InfectionEventArgs : EventArgs
    {
        public InfectionEventArgs(City city)
        {
            City = city;
        }

        public City City { get; }
    }

    public class InfoTextEventArgs : EventArgs
    {
        public InfoTextEventArgs(string infoText)
        {
            InfoText = infoText;
        }

        public string InfoText { get; }
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

    public class ShareTypeEventArgs : EventArgs
    {
        public ShareTypeEventArgs(IEnumerable<ShareType> shareTypes, Action<ShareType> selectionDelegate)
        {
            SelectionDelegate = selectionDelegate ?? throw new ArgumentNullException(nameof(selectionDelegate));
            ShareTypes = shareTypes ?? throw new ArgumentNullException(nameof(shareTypes));
        }

        public Action<ShareType> SelectionDelegate { get; }
        public IEnumerable<ShareType> ShareTypes { get; }
    }
}
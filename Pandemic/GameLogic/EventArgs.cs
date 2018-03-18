using System;
using System.Collections.Generic;

namespace Pandemic
{
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

    public class InfectionEventArgs
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

    public class OutbreakEventArgs
    {
        public OutbreakEventArgs(City city)
        {
            City = city;
        }

        public City City { get; }
    }
}
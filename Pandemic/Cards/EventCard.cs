using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Pandemic.Cards
{

    public abstract class EventCard : Card
    {
        public EventCard(string name) : base(name)
        { }

        public Character Character { get; set; }

        public abstract void PlayEvent(Game game);

        public event EventHandler EventFinished;

        protected virtual void OnEventFinished(EventArgs e, Game game)
        {
            Character.RemoveCard(this);
            game.AddCardToPlayerDiscardPile(this);
            EventFinished?.Invoke(this, e);
        }
    }
}
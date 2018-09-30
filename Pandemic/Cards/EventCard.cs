using Pandemic.GameLogic.Actions;
using System;

namespace Pandemic.Cards
{
    public class EventCard : Card
    {
        private Game _game;

        public EventCard(string name, Func<Game, EventCard, EventAction> createEventAction) : base(name)
        {
            CreateEventAction = createEventAction ?? throw new ArgumentNullException(nameof(createEventAction));
        }

        public event EventHandler EventFinished;

        public Func<Game, EventCard, EventAction> CreateEventAction { get; }
        public Character Character { get; set; }

        public void PlayEvent(Game game)
        {
            _game = game;
            CreateEventAction(game, this).Execute();
        }

        protected virtual void OnEventFinished(EventArgs e)
        {
            EventFinished?.Invoke(this, e);
        }

        public void FinishEvent()
        {
            Character.RemoveCard(this);
            _game.AddCardToPlayerDiscardPile(this);
            OnEventFinished(EventArgs.Empty);
        }
    }
}
using Pandemic.GameLogic.Actions;
using System;

namespace Pandemic.Cards
{
    public class EventCard : Card
    {
        private Game _game;

        public EventCard(string name, Func<EventAction> factoryMethod) : base(name)
        {
            FactoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
        }

        public event EventHandler EventFinished;

        public Func<EventAction> FactoryMethod { get; }
        public Character Character { get; set; }

        public void PlayEvent(Game game)
        {
            _game = game;
            FactoryMethod().Execute(game, FinishEvent);
        }

        protected virtual void OnEventFinished(EventArgs e)
        {
            EventFinished?.Invoke(this, e);
        }

        private void FinishEvent()
        {
            Character.RemoveCard(this);
            _game.AddCardToPlayerDiscardPile(this);
            OnEventFinished(EventArgs.Empty);
        }
    }
}
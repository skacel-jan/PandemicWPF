using Pandemic.Cards;
using System;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public abstract class EventAction
    {
        protected EventAction(EventCard card, Game game)
        {
            EventCard = card ?? throw new ArgumentNullException(nameof(card));
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public string Name => ActionTypes.Event;

        protected EventCard EventCard { get; }
        protected Game Game { get; private set; }

        public bool CanExecute() => true;

        public abstract void Execute();

        protected void ShowGameInfo(string text)
        {
            Game.Info = new GameInfo(text);
        }

        protected void ShowGameInfo(string text, string actionText, Action action)
        {
            Game.Info = new GameInfo(text, actionText, action);
        }

        protected void HideGameInfo()
        {
            Game.Info = null;
        }

        protected virtual void FinishAction()
        {
            HideGameInfo();
            EventCard.FinishEvent();
        }
    }
}
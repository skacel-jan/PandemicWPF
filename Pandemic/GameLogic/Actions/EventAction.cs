using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public abstract class EventAction : IGameAction
    {
        protected EventAction(EventCard card, Game game)
        {
            EventCard = card ?? throw new ArgumentNullException(nameof(card));
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public string Name => ActionTypes.Event;

        protected EventCard EventCard { get; }
        protected Game Game { get; private set; }

        protected IList<IEffect> Effects { get; } = new List<IEffect>();

        protected Queue<Selection> Selections { get; private set; }

        public void Execute()
        {
            if (Selections == null)
            {
                PrepareSelections();
            }

            if (Selections.Any())
            {
                Game.ResolveSelection(Selections.Dequeue());
            }
            else
            {
                AddEffects();
                if (EventCard.IsReturnedByContingencyPlanner)
                {
                    Effects.Add(new RemoveEventCardEffect(Game.RemovedCards, EventCard));
                }
                else
                {
                    Effects.Add(new DiscardEventCardEffect(Game.PlayerDiscardPile, EventCard));
                }
                

                foreach (var effect in Effects)
                {
                    effect.Execute();
                }

                Selections = null;
                Game.Continue();
            }
        }

        protected Action<T> SetSelectionCallback<T>(Action<T> action)
        {
            return action += (_) => Execute();
        }

        protected abstract void AddEffects();

        public virtual bool CanExecute() => true;

        protected abstract IEnumerable<Selection> PrepareSelections(Game game);

        public void PrepareSelections()
        {            
            Selections = new Queue<Selection>(PrepareSelections(Game));
        }
    }
}
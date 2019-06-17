using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public abstract class CharacterAction : IGameAction
    {
        protected CharacterAction(Character character, Game game)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public abstract string Name { get; }
        public Character Character { get; }
        public Game Game { get; }
        protected Queue<Selection> Selections { get; private set; }
        protected IList<IEffect> Effects { get; private set; } = new List<IEffect>();

        public void Execute()
        {
            PrepareSelections();

            Continue();
        }

        private void Continue()
        {
            if (Selections.Any())
            {
                Game.ResolveSelection(Selections.Dequeue());
            }
            else
            {
                Effects.Clear();
                AddEffects();
                Effects.Add(new CharacterActionFinishedEffect(Game));

                foreach (var effect in Effects)
                {
                    Game.ResolveEffect(effect);
                }

                Selections = null;
                Game.Continue();
            }
        }

        protected Action<T> SetSelectionCallback<T>(Action<T> action)
        {
            return action += (_) => Continue();
        }

        protected abstract void AddEffects();

        public virtual bool CanExecute() => true;

        protected abstract IEnumerable<Selection> PrepareSelections(Game game);

        private void PrepareSelections()
        {
            Selections = new Queue<Selection>(PrepareSelections(Game));
        }
    }
}
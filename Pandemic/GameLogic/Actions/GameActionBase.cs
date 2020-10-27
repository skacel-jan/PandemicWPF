using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public abstract class GameActionBase : IGameAction
    {
        protected GameActionBase(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public abstract string Name { get; }
        public Game Game { get; }
        protected IList<IEffect> Effects { get; private set; } = new List<IEffect>();

        private readonly Dictionary<int, IList<ActionState>> _minorActions = new Dictionary<int, IList<ActionState>>();

        protected int m_internalState = -1;

        protected void AddContinueState(int state, Action<Game> action)
        {
            AddContinueState(state, (g) => true, action);
        }

        protected void AddContinueState(int state, Predicate<Game> predicate, Action<Game> action)
        {
            action += (_) => Continue();
            AddState(state, new ActionState(predicate, action, Game));
        }

        protected void AddState(int state, ActionState action)
        {
            if (!_minorActions.ContainsKey(state))
            {
                _minorActions[state] = new List<ActionState>();
            }

            _minorActions[state].Add(action);
        }

        protected void AddSelectionState(int state, Selection selection)
        {
            AddSelectionState(state, (g) => true, selection);
        }

        protected void AddSelectionState(int state, Predicate<Game> predicate, Selection selection)
        {
            AddState(state, new ActionState(predicate, (g) => g.ResolveSelection(selection), Game));
        }

        protected void AddSelectionState(int state, Predicate<Game> predicate, Func<Selection> selectionGetter)
        {
            AddState(state, new ActionState(predicate, (g) => g.ResolveSelection(selectionGetter()), Game));
        }

        public void Execute()
        {
            _minorActions.Clear();
            Effects.Clear();
            Initialize();
            m_internalState = -1;
            Continue();
        }

        protected void Continue()
        {
            m_internalState++;
            if (_minorActions.TryGetValue(m_internalState, out var actions))
            {
                foreach (var action in actions)
                {
                    if (action.Predicate(Game))
                    {
                        action.Execute();
                    }
                }
            }
            else
            {
                AddEffects();

                foreach (var effect in Effects)
                {
                    Game.ResolveEffect(effect);
                }
                Game.Continue();
            }
        }

        protected Action<T> SelectionCallback<T>(Action<T> action)
        {
            return action += (_) => Continue();
        }

        protected abstract void AddEffects();

        public virtual bool CanExecute() => true;

        protected abstract void Initialize();
    }
}
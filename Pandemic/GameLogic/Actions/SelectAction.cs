using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public class SelectAction<T> : ISelectAction<T>
    {
        public SelectAction(Action<T> action, IEnumerable<T> items, string text)
           : this(action, items, text, (T) => true)
        {
        }

        public SelectAction(Action<T> action, IEnumerable<T> items, string text, Predicate<T> predicate)
        {
            Action = action;
            Items = items;
            Text = text;
            Predicate = predicate;
        }

        public Action<T> Action { get; }

        public IEnumerable<T> Items { get; }

        public Predicate<T> Predicate { get; }
        public string Text { get; }

        public bool CanExecute(object param)
        {
            return Predicate((T)param);
        }

        public void Execute(object param)
        {
            Action((T)param);
        }
    }

    public class MultiSelectAction<T> : IMultiSelectAction<T>
    {
        public MultiSelectAction(Action<T> action, T items, string text)
           : this(action, items, text, (T) => true)
        {
        }

        public MultiSelectAction(Action<T> action, T items, string text, Predicate<T> predicate)
        {
            Action = action;
            Items = items;
            Text = text;
            Predicate = predicate;
        }

        public Action<T> Action { get; }

        public T Items { get; }

        public Predicate<T> Predicate { get; }
        public string Text { get; }

        public bool CanExecute(object param)
        {
            return Predicate((T)param);
        }

        public void Execute(object param)
        {
            Action((T)param);
        }
    }
}
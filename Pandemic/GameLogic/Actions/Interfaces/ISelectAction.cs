using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public interface IAction
    {
        bool CanExecute(object param);

        void Execute(object param);
    }

    public interface ISelectAction<T> : IAction
    {
        IEnumerable<T> Items { get; }
        string Text { get; }
    }

    public interface IMultiSelectAction<T> : IAction
    {
        T Items { get; }
        string Text { get; }
    }
}
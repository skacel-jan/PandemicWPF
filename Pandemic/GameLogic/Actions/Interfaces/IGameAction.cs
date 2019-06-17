using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public interface IGameAction
    {
        string Name { get; }

        bool CanExecute();

        void Execute();
    }
}
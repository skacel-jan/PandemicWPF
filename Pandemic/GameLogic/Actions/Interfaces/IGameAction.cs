using System;

namespace Pandemic.GameLogic.Actions
{
    public interface IGameAction
    {
        string Name { get; }

        bool CanExecute(Game game);

        void Execute(Game game, Action callbackAction);
    }
}
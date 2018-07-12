using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    public interface IMoveAction
    {
        bool IsCardRequired { get; }

        bool IsPossible(MapCity city);

        void Move(Game game, MapCity city, Action finishAction);
    }
}
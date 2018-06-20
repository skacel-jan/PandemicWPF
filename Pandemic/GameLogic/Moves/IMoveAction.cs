using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IMoveAction
    {
        string MoveType { get; }

        bool IsCardRequired { get; }

        bool IsPossible(MapCity city);

        void Move(Game game, MapCity city, Action finishAction);
    }
}
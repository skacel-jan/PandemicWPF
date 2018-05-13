using Pandemic.Cards;
using System;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IMoveAction
    {
        string MoveType { get; }

        bool IsPossible(MapCity city);

        bool Move(MapCity city);
    }

    public interface IMoveCardAction
    {
        string MoveType { get; }

        bool IsPossible(MapCity city);

        bool Move(MapCity city, PlayerCard card);
    }
}
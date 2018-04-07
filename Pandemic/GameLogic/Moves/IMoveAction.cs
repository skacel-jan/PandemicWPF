using System;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IMoveAction
    {
        Character Character { get; set; }
        string MoveType { get; }

        bool IsPossible(MapCity city);

        bool Move(MapCity city);
    }

    public interface IMoveCardAction : IMoveAction
    {
        PlayerCard Card { get; set; }
    }
}
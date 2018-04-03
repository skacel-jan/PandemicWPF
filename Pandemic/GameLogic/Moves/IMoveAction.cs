using System;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IMoveAction
    {
        Character Character { get; set; }
        bool IsCardNeeded { get; }
        string MoveType { get; }

        bool CanMove(MapCity city);

        bool Move(MapCity city);
    }
}
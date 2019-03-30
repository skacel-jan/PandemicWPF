using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    interface IGameComponents
    {
        Infection Infection { get; }
        WorldMap WorldMap { get; }
        CircularCollection<Character> Characters { get; }

    }
}

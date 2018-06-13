using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic
{
    public interface IGamePhase
    {
        void Start();
        void End();
        void Action(string actionType);
    }
}

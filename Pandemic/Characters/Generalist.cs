using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class Generalist : Character
    {
        private const int GENERALIST_ACTIONS_COUNT = 5;

        public override int ActionsCount => GENERALIST_ACTIONS_COUNT;

        public override string Role => "Generalist";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class OperationsExpert : Character
    {
        public override string Role => "Operations expert";

        private IEnumerable<string> _roleDescription = new List<string>()
        {
            "As an action, build research station in the city you are in (no discard needed).",
            "Once prer turn as an action, move from a research station to any city by discarding any City card."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override Color Color => Colors.Green;
    }
}

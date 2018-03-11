using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Dispatcher : Character
    {
        public override string Role => "Disptacher";

        public override IEnumerable<string> RoleDescription => new List<string>()
        {
            "Move another player´s pawn as if it were yours.",
            "As an action, move any pawn to a city with another pawn."
        };

        public override Color Color => Colors.Pink;
    }
}

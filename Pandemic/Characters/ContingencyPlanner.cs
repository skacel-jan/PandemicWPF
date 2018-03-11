using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class ContingencyPlanner : Character
    {
        public override string Role => "Contingency player";

        public override IEnumerable<string> RoleDescription => new List<string>()
        {
            "As an action, take any discarded Event card and store it on this card.",
            "When you play the stored Event card, remove it from the game."
        };

        public override Color Color => Colors.LightSkyBlue;
    }
}

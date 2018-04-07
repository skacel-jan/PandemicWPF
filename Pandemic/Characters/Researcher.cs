using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Researcher : Character
    {
        public override Color Color => Colors.Brown;

        public override string Role => "Researcher";

        private IEnumerable<string> _roleDescription = new List<string>()
        {
            @"When doing the Share Knowledge action, the Researcher
              may give any City card from her hand to another player
              in the same city as her, without this card having to match
              her city. The transfer must be from her hand to the other
              player’s hand, but it can occur on either player’s turn."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;
    }
}

using System.Collections.Generic;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Scientist : Character
    {
        public const string SCIENTIST = "Scientist";

        public override int CardsForCure { get => 4; }

        public override Color Color => Colors.White;
        public override string Role => SCIENTIST;

        private IEnumerable<string> _roleDescription = new List<string>()
        {
            "You need only 4 cards of the same color to do the Discover a Cure action.",
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;
    }
}
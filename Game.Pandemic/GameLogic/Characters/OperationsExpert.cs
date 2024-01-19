using System.Collections.Generic;
using System.Windows.Media;

namespace Game.Pandemic.GameLogic.Characters
{
    public class OperationsExpert : Character
    {
        public const string OPERATIONS_EXPERT = "Operations expert";
        public override string Role => OPERATIONS_EXPERT;

        private readonly IEnumerable<string> _roleDescription = new List<string>()
        {
            "As an action, build research station in the city you are in (no discard needed).",
            "Once per turn as an action, move from a research station to any city by discarding any City card."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override Color Color => Colors.Green;
    }
}
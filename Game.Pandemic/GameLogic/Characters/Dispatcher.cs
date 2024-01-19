using System.Collections.Generic;
using System.Windows.Media;

namespace Game.Pandemic.GameLogic.Characters
{
    public class Dispatcher : Character
    {
        public const string DISPATCHER = "Dispatcher";
        public override string Role => DISPATCHER;

        public override IEnumerable<string> RoleDescription => new List<string>()
        {
            "Move another player´s pawn as if it were yours.",
            "As an action, move any pawn to a city with another pawn."
        };

        public override Color Color => Colors.Pink;
    }
}
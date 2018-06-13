﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class OperationsExpert : Character
    {
        public const string ROLE = "Operations expert";
        public override string Role => ROLE;

        private IEnumerable<string> _roleDescription = new List<string>()
        {
            "As an action, build research station in the city you are in (no discard needed).",
            "Once per turn as an action, move from a research station to any city by discarding any City card."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override Color Color => Colors.Green;

        public OperationsExpert() : base()
        {
            MoveStrategy = new OperationsExpertMoveStrategy(this);
        }
    }
}

﻿using System.Collections.Generic;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class ContingencyPlanner : Character
    {
        public const string CONTINGENCY_PLANNER = "Contingency planner";

        public override string Role => CONTINGENCY_PLANNER;

        public override IEnumerable<string> RoleDescription => new List<string>()
        {
            "As an action, take any discarded Event card and store it on this card.",
            "When you play the stored Event card, remove it from the game."
        };

        public override Color Color => Colors.LightSkyBlue;
    }
}
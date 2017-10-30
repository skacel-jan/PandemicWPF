using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy.Characters
{
    public class Scientist : Character
    {
        public override int CardsForCure { get => 4; }

        public override bool CanDiscoverCure(Disease disease)
        {
            return this.CurrentMapCity.HasResearchStation && this.Player.SameColorCards(disease) >= CardsForCure;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy.Characters
{
    public class Scientist : Character
    {
        private const int SCIENTIST_CARDS_FOR_CURE = 5;

        public override bool CanDiscoverCure(Disease disease)
        {
            return this.Pawn.MapCity.HasReaserchStation && this.Player.SameColorCards(disease) >= SCIENTIST_CARDS_FOR_CURE;
        }
    }
}

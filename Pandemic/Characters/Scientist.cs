using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Characters
{
    public class Scientist : Character
    {
        public override int CardsForCure { get => 4; }

        public override string Role => "Scientist";

        public override bool CanDiscoverCure(DiseaseColor disease)
        {
            return CurrentMapCity.HasResearchStation && Player.ColorCardsCount(disease) >= CardsForCure;
        }
    }
}

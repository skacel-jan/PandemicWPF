using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy.Characters
{
    public class Medic : Character
    {
        public override void DriveOrFerry(MapCity toCity)
        {
            base.DriveOrFerry(toCity);
            SpecialTreatDisease();
        }

        public override void TreatDisease(Disease disease)
        {
            this.Pawn.MapCity.RemoveInfection(disease.Color);
        }

        protected virtual void SpecialTreatDisease()
        {
            foreach (var disease in Common.Diseases.Values)
            {
                if (disease.KnownCure)
                    this.Pawn.MapCity.RemoveInfection(disease.Color);
            }
        }
    }
}

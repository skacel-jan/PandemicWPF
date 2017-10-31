using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Characters
{
    public class Medic : Character
    {
        public override void DriveOrFerry(MapCity toCity)
        {
            base.DriveOrFerry(toCity);
            SpecialTreatDisease();
        }

        public override int TreatDisease(Disease disease)
        {
            this.CurrentMapCity.RemoveInfection(disease.Color);
            return 3;
        }

        protected virtual void SpecialTreatDisease()
        {
            //foreach (var disease in Enum.GetValues(typeof(DiseaseColor)))
            //{
            //    if (disease.KnownCure)
            //        this.CurrentMapCity.RemoveInfection(disease.Color);
            //}
        }
    }
}

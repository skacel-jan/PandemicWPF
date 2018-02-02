using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Characters
{
    public class Medic : Character
    {
        public override string Role => "Medic";

        public override void DriveOrFerry(MapCity toCity)
        {
            base.DriveOrFerry(toCity);
            SpecialTreatDisease();
        }

        public override PlayerCard CharterFlight(MapCity toCity)
        {
            PlayerCard card = base.CharterFlight(toCity);
            SpecialTreatDisease();
            return card;
        }

        public override void ShuttleFlight(MapCity toCity)
        {
            base.ShuttleFlight(toCity);
            SpecialTreatDisease();
        }

        public override PlayerCard DirectFlight(MapCity toCity)
        {
            PlayerCard card = base.DirectFlight(toCity);
            SpecialTreatDisease();
            return card;
        }

        public override int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.RemoveInfection(diseaseColor);
        }

        protected virtual void SpecialTreatDisease()
        {
            CurrentMapCity.RemoveCuredInfections();
        }
    }
}

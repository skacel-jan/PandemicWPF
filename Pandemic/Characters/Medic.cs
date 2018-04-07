using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Medic : Character
    {
        public override string Role => "Medic";

        private IEnumerable<string> _roleDescription = new List<string>()
        {
            "Remove all cubes of one color when doing Treat Disease",
            "Automatically remove cubes of cured diseases from a city you are in (and prevent them from being placed there)."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override Color Color => Colors.Orange;

        public override bool Move(string moveType, MapCity city)
        {
            bool result = base.Move(moveType, city);
            if (result)
            {
                SpecialTreatDisease();
            }            

            return result;
        }

        public override int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.RemoveInfection(diseaseColor);
        }

        protected virtual void SpecialTreatDisease()
        {
            CurrentMapCity.RemoveCuredInfections();
        }

        public override bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return city.Diseases[color].IsCured;
        }

        public override void RegisterSpecialActions(SpecialActions actions)
        {
            base.RegisterSpecialActions(actions);

            actions.DiseaseCuredActions.Add((color) => SpecialTreatDisease());
        }
    }
}

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
        public const string ROLE = "Medic";

        public override string Role => ROLE;

        private readonly IEnumerable<string> _roleDescription = new List<string>()
        {
            "Remove all cubes of one color when doing Treat Disease",
            "Automatically remove cubes of cured diseases from a city you are in (and prevent them from being placed there)."
        };

        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override Color Color => Colors.Orange;

        public override bool Move(string moveType, MapCity city)
        {
            bool moveSucessfull = base.Move(moveType, city);

            if (moveSucessfull)
            {
                SpecialTreatDisease();
            }

            return moveSucessfull;
        }

        public override int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.RemoveInfection(diseaseColor);
        }

        public virtual void SpecialTreatDisease()
        {
            foreach (var disease in CurrentMapCity.Diseases.Values)
            {
                if (disease.IsCured)
                {
                    CurrentMapCity.RemoveInfection(disease.Color);
                }
            }
        }

        public override bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return city.Diseases[color].IsCured;
        }
    }
}

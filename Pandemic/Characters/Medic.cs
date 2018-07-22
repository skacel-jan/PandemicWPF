using Pandemic.GameLogic;
using Pandemic.GameLogic.Actions;
using System.Collections.Generic;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Medic : Character
    {
        public const string MEDIC = "Medic";

        private readonly IEnumerable<string> _roleDescription = new List<string>()
        {
            "Remove all cubes of one color when doing Treat Disease",
            "Automatically remove cubes of cured diseases from a city you are in (and prevent them from being placed there)."
        };

        public Medic()
        {
            Actions[ActionTypes.Move] = new MedicMoveAction(this);
        }

        public override Color Color => Colors.Orange;
        public override string Role => MEDIC;
        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return city.Diseases[color].Status > Disease.State.NotCured;
        }

        public virtual void SpecialTreatDisease()
        {
            foreach (var disease in CurrentMapCity.Diseases.Values)
            {
                if (disease.Status > Disease.State.NotCured)
                {
                    CurrentMapCity.RemoveInfection(disease.Color);
                }
            }
        }

        public override int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.RemoveInfection(diseaseColor);
        }
    }
}
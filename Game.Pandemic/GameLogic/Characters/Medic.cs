using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Board;

namespace Game.Pandemic.GameLogic.Characters
{
    public class Medic : Character
    {
        public const string MEDIC = "Medic";

        private readonly IEnumerable<string> _roleDescription = new List<string>()
        {
            "Remove all cubes of one color when doing Treat Disease",
            "Automatically remove cubes of cured diseases from a city you are in (and prevent them from being placed there)."
        };

        public override Color Color => Colors.Orange;
        public override string Role => MEDIC;
        public override IEnumerable<string> RoleDescription => _roleDescription;

        public override bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return city.Diseases[color].Status > Disease.State.NotCured;
        }

        public override int TreatDisease(DiseaseColor diseaseColor)
        {
            return CurrentMapCity.RemoveInfection(diseaseColor);
        }

        public override IEnumerable<IEffect> GetMoveEffects(MapCity city, Game game)
        {
            return base.GetMoveEffects(city, game).Concat(GetSpecialEffects());

            IEnumerable<IEffect> GetSpecialEffects()
            {
                foreach (var disease in CurrentMapCity.Diseases.Values)
                {
                    if (disease.Status > Disease.State.NotCured)
                    {
                        yield return new TreatDiseaseEffect(game, disease.Color, this);
                    }
                }
            }
        }
    }
}
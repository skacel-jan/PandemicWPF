using Pandemic.GameLogic;
using System.Collections.Generic;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class QuarantineSpecialist : Character
    {
        public const string QUARANTINE_SPECIALIST = "Quarantine specialist";
        public override string Role => QUARANTINE_SPECIALIST;

        public override IEnumerable<string> RoleDescription => new List<string>()
        {
            "Prevent disease cube placements (and outbreaks) in the city you are in and cities connected to it."
        };

        public override Color Color => Colors.DarkGreen;

        public override bool CanPreventInfection(MapCity city, DiseaseColor color)
        {
            return ((CurrentMapCity == city) || (CurrentMapCity.IsCityConnected(city)));
        }
    }
}
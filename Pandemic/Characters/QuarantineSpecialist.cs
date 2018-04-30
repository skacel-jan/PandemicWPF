using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class QuarantineSpecialist : Character
    {
        public override string Role => "Quarantine specialist";

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

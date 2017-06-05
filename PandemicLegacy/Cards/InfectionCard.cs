using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public class InfectionCard : CityCard
    {
        public InfectionCard(City city) : base(city)
        {

        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", "Infection",  base.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class CityCard : Card
    {
        public City City { get; private set; }

        public CityCard(City city) : base(city.Name)
        {
            City = city ?? throw new ArgumentNullException(nameof(city));
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", City.Name, City.Color);
        }
    }
}

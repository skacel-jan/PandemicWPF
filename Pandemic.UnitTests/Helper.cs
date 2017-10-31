using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.UnitTests
{
    public static class Helper
    {
        public static IEnumerable<City> GetCities()
        {
            var factory = new WorldMapFactory();
            return factory.GetCities();
        }

        public static IEnumerable<InfectionCard> GetInfectionCards()
        {
            return GetCities().Select(x => new InfectionCard(x));
        }

        public static IEnumerable<Card> GetPlayerCards()
        {
            return GetCities().Select(x => new PlayerCard(x));
        }

    }
}

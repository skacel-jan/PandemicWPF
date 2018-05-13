using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class WorldMap
    {
        public IDictionary<string, MapCity> Cities { get; }

        public WorldMap(IDictionary<string, MapCity> cities)
        {
            Cities = cities ?? throw new ArgumentNullException(nameof(cities));
        }

        public MapCity GetCity(string city)
        {
            return Cities[city];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class WorldMap
    {
        public IDictionary<string, MapCity> Cities { get; private set; }

        public WorldMap(IDictionary<string, MapCity> cities)
        {
            this.Cities = cities;
        }

        public MapCity GetCity(string city)
        {
            return Cities[city];
        }
    }
}

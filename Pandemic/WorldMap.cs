using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class WorldMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public IDictionary<string, MapCity> Cities { get; private set; }

        public WorldMap(IDictionary<string, MapCity> cities)
        {
            this.Cities = cities;
        }

        public WorldMap(IDictionary<string, MapCity> cities, int width, int height)
        {
            this.Cities = cities;
            this.Width = width;
            this.Height = height;
        }

        public MapCity GetCity(string city)
        {
            return Cities[city];
        }
    }
}

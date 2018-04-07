using System.Collections.Generic;

namespace Pandemic
{
    public interface IWorldMapFactory
    {
        IList<City> Cities { get; }
        IDictionary<string, MapCity> MapCities { get; }
        WorldMap WorldMap { get; }
    }
}
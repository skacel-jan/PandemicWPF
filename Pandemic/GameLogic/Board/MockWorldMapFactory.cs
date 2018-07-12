using System.Collections.Generic;

namespace Pandemic.GameLogic
{
    internal class MockWorldMapFactory : IWorldMapFactory
    {
        public WorldMap CreateWorldMap(IDictionary<DiseaseColor, Disease> diseases)
        {
            return new WorldMap(new Dictionary<string, MapCity>()
            {
                { City.Atlanta, new MapCity(new City(City.Atlanta, DiseaseColor.Blue), diseases) },
                { City.Algiers, new MapCity(new City(City.Algiers, DiseaseColor.Blue), diseases) },
                { City.Baghdad, new MapCity(new City(City.Baghdad, DiseaseColor.Blue), diseases) },
                { City.Bangkok, new MapCity(new City(City.Bangkok, DiseaseColor.Blue), diseases) },
                { City.Beijing, new MapCity(new City(City.Beijing, DiseaseColor.Blue), diseases) },
                { City.Bogota, new MapCity(new City(City.Bogota, DiseaseColor.Blue), diseases) },
                { City.BuenosAires, new MapCity(new City(City.BuenosAires, DiseaseColor.Blue), diseases) },
                { City.Cairo, new MapCity(new City(City.Cairo, DiseaseColor.Blue), diseases) },
                { City.Chennai, new MapCity(new City(City.Chennai, DiseaseColor.Blue), diseases) },
                { City.Chicago, new MapCity(new City(City.Chicago, DiseaseColor.Blue), diseases) },
                { City.Delhi, new MapCity(new City(City.Delhi, DiseaseColor.Blue), diseases) },
                { City.Essen, new MapCity(new City(City.Essen, DiseaseColor.Blue), diseases) },
                { City.HoChiMinhCity, new MapCity(new City(City.HoChiMinhCity, DiseaseColor.Blue), diseases) }
            });
        }
    }
}
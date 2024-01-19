using System.Collections.Generic;
using System.Linq;

namespace Game.Pandemic.GameLogic.Board
{
    internal class MockWorldMapFactory : IWorldMapFactory
    {
        public WorldMap CreateWorldMap(IDictionary<DiseaseColor, Disease> diseases)
        {
            return new WorldMap(GetCities().Select(c => new MapCity(c, diseases)));
        }

        private IEnumerable<City> GetCities()
        {
            return new List<City>()
            {
                new City(City.Atlanta, DiseaseColor.Blue),
                new City(City.Algiers, DiseaseColor.Black),
                new City(City.Baghdad, DiseaseColor.Black),
                new City(City.Bangkok, DiseaseColor.Red),
                new City(City.Beijing, DiseaseColor.Red),
                new City(City.Bogota, DiseaseColor.Yellow),
                new City(City.BuenosAires, DiseaseColor.Yellow),
                new City(City.Cairo, DiseaseColor.Black),
                new City(City.Chennai, DiseaseColor.Black),
                new City(City.Chicago, DiseaseColor.Blue),
                new City(City.Delhi, DiseaseColor.Black),
                new City(City.Essen, DiseaseColor.Blue),
                new City(City.HoChiMinhCity, DiseaseColor.Red),
                new City(City.HongKong, DiseaseColor.Red),
                new City(City.Istanbul, DiseaseColor.Black),
                new City(City.Jakarta, DiseaseColor.Red),
                new City(City.Johannesburg, DiseaseColor.Yellow),
                new City(City.Karachi, DiseaseColor.Black),
                new City(City.Khartoum, DiseaseColor.Yellow),
                new City(City.Kinshasa, DiseaseColor.Yellow),
                new City(City.Kolkata, DiseaseColor.Black),
                new City(City.Lagos, DiseaseColor.Yellow),
                new City(City.Lima, DiseaseColor.Yellow),
                new City(City.London, DiseaseColor.Blue),
                new City(City.LosAngeles, DiseaseColor.Yellow),
                new City(City.Madrid, DiseaseColor.Blue),
                new City(City.Manila, DiseaseColor.Red),
                new City(City.MexicoCity, DiseaseColor.Yellow),
                new City(City.Miami, DiseaseColor.Yellow),
                new City(City.Milan, DiseaseColor.Blue),
                new City(City.Montreal, DiseaseColor.Blue),
                new City(City.Moscow, DiseaseColor.Black),
                new City(City.Mumbai, DiseaseColor.Black),
                new City(City.NewYork, DiseaseColor.Blue),
                new City(City.Osaka, DiseaseColor.Red),
                new City(City.Paris, DiseaseColor.Blue),
                new City(City.Riyadh, DiseaseColor.Black),
                new City(City.SanFrancisco, DiseaseColor.Blue),
                new City(City.Santiago, DiseaseColor.Yellow),
                new City(City.SaoPaulo, DiseaseColor.Yellow),
                new City(City.Seoul, DiseaseColor.Red),
                new City(City.Shanghai, DiseaseColor.Red),
                new City(City.StPetersburg, DiseaseColor.Blue),
                new City(City.Sydney, DiseaseColor.Red),
                new City(City.Taipei, DiseaseColor.Red),
                new City(City.Tehran, DiseaseColor.Black),
                new City(City.Tokyo, DiseaseColor.Red),
                new City(City.Washington, DiseaseColor.Blue)
            };
        }
    }
}
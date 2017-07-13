using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public class WorldMapFactory
    {
        public WorldMap BuildMap()
        {
            var mapCities = new Dictionary<string, MapCity>(GetCities().ToDictionary(x => x.Name, x => new MapCity(x)));

            mapCities[City.Algiers].AddConnectedCities(mapCities[City.Cairo], mapCities[City.Istanbul], mapCities[City.Madrid], mapCities[City.Paris]);
            mapCities[City.Atlanta].AddConnectedCities(mapCities[City.Chicago], mapCities[City.Miami], mapCities[City.Washington]);
            mapCities[City.Baghdad].AddConnectedCities(mapCities[City.Cairo], mapCities[City.Istanbul], mapCities[City.Tehran], mapCities[City.Riyadh]);
            mapCities[City.Bangkok].AddConnectedCities(mapCities[City.Jakarta], mapCities[City.HoChiMinhCity], mapCities[City.HongKong], mapCities[City.Kolkata]);
            mapCities[City.Beijing].AddConnectedCities(mapCities[City.Shanghai], mapCities[City.Seoul]);
            mapCities[City.Bogota].AddConnectedCities(mapCities[City.BuenosAires], mapCities[City.MexicoCity], mapCities[City.Miami], mapCities[City.Lima], mapCities[City.SaoPaulo]);
            mapCities[City.Cairo].AddConnectedCities(mapCities[City.Algiers], mapCities[City.Baghdad], mapCities[City.Istanbul], mapCities[City.Khartoum], mapCities[City.Riyadh]);
            mapCities[City.Chennai].AddConnectedCities(mapCities[City.Delhi], mapCities[City.Jakarta], mapCities[City.Kolkata], mapCities[City.Mumbai]);
            mapCities[City.Chicago].AddConnectedCities(mapCities[City.Atlanta], mapCities[City.LosAngeles], mapCities[City.MexicoCity], mapCities[City.Montreal], mapCities[City.SanFrancisco]);
            mapCities[City.Delhi].AddConnectedCities(mapCities[City.Chennai], mapCities[City.Karachi], mapCities[City.Kolkata], mapCities[City.Mumbai], mapCities[City.Tehran]);
            mapCities[City.Essen].AddConnectedCities(mapCities[City.London], mapCities[City.Milan], mapCities[City.Paris], mapCities[City.StPetersburg]);
            mapCities[City.HoChiMinhCity].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.Jakarta], mapCities[City.HongKong], mapCities[City.Manila]);
            mapCities[City.HongKong].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.HoChiMinhCity], mapCities[City.Kolkata], mapCities[City.Manila], mapCities[City.Shanghai], mapCities[City.Taipei]);
            mapCities[City.HoChiMinhCity].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.Jakarta], mapCities[City.HongKong], mapCities[City.Manila]);
            mapCities[City.Istanbul].AddConnectedCities(mapCities[City.Algiers], mapCities[City.Baghdad], mapCities[City.Cairo], mapCities[City.Milan], mapCities[City.Moscow]);
            mapCities[City.Jakarta].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.Chennai], mapCities[City.HoChiMinhCity], mapCities[City.Sydney]);
            mapCities[City.Johannesburg].AddConnectedCities(mapCities[City.BuenosAires], mapCities[City.Kinshasa], mapCities[City.Khartoum]);
            mapCities[City.Karachi].AddConnectedCities(mapCities[City.Delhi], mapCities[City.Mumbai], mapCities[City.Riyadh], mapCities[City.Tehran]);
            mapCities[City.Khartoum].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.Jakarta], mapCities[City.HongKong], mapCities[City.Manila]);
            mapCities[City.Kinshasa].AddConnectedCities(mapCities[City.Johannesburg], mapCities[City.Lagos], mapCities[City.Khartoum]);
            mapCities[City.Kolkata].AddConnectedCities(mapCities[City.Bangkok], mapCities[City.Chennai], mapCities[City.Delhi], mapCities[City.HongKong]);
            mapCities[City.Lagos].AddConnectedCities(mapCities[City.Kinshasa], mapCities[City.Khartoum], mapCities[City.SaoPaulo]);
            mapCities[City.Lima].AddConnectedCities(mapCities[City.Bogota], mapCities[City.MexicoCity], mapCities[City.LosAngeles], mapCities[City.Santiago]);
            mapCities[City.London].AddConnectedCities(mapCities[City.Essen], mapCities[City.Madrid], mapCities[City.NewYork], mapCities[City.Paris]);
            mapCities[City.LosAngeles].AddConnectedCities(mapCities[City.Chicago], mapCities[City.MexicoCity], mapCities[City.SanFrancisco], mapCities[City.Sydney]);
            mapCities[City.Madrid].AddConnectedCities(mapCities[City.Algiers], mapCities[City.London], mapCities[City.NewYork], mapCities[City.SaoPaulo]);
            mapCities[City.Manila].AddConnectedCities(mapCities[City.HoChiMinhCity], mapCities[City.HongKong], mapCities[City.SanFrancisco], mapCities[City.Sydney], mapCities[City.Taipei]);
            mapCities[City.MexicoCity].AddConnectedCities(mapCities[City.Bogota], mapCities[City.Chicago], mapCities[City.Lima], mapCities[City.LosAngeles], mapCities[City.Miami]);
            mapCities[City.Miami].AddConnectedCities(mapCities[City.Atlanta], mapCities[City.Bogota], mapCities[City.MexicoCity], mapCities[City.Washington]);
            mapCities[City.Milan].AddConnectedCities(mapCities[City.Essen], mapCities[City.Istanbul], mapCities[City.Paris]);
            mapCities[City.Montreal].AddConnectedCities(mapCities[City.Chicago], mapCities[City.NewYork], mapCities[City.Washington]);
            mapCities[City.Moscow].AddConnectedCities(mapCities[City.Istanbul], mapCities[City.StPetersburg], mapCities[City.Tehran]);
            mapCities[City.Mumbai].AddConnectedCities(mapCities[City.Chennai], mapCities[City.Delhi], mapCities[City.Karachi]);
            mapCities[City.NewYork].AddConnectedCities(mapCities[City.London], mapCities[City.Madrid], mapCities[City.Montreal], mapCities[City.Washington]);
            mapCities[City.Osaka].AddConnectedCities(mapCities[City.Taipei], mapCities[City.Tokyo]);
            mapCities[City.Paris].AddConnectedCities(mapCities[City.Algiers], mapCities[City.Essen], mapCities[City.London], mapCities[City.Madrid], mapCities[City.Milan]);
            mapCities[City.Riyadh].AddConnectedCities(mapCities[City.Baghdad], mapCities[City.Cairo], mapCities[City.Karachi]);
            mapCities[City.SanFrancisco].AddConnectedCities(mapCities[City.Chicago], mapCities[City.LosAngeles], mapCities[City.Manila], mapCities[City.Tokyo]);
            mapCities[City.Santiago].AddConnectedCities(mapCities[City.BuenosAires], mapCities[City.Lima]);
            mapCities[City.SaoPaulo].AddConnectedCities(mapCities[City.Bogota], mapCities[City.BuenosAires], mapCities[City.Lagos], mapCities[City.Madrid]);
            mapCities[City.Seoul].AddConnectedCities(mapCities[City.Beijing], mapCities[City.Shanghai], mapCities[City.Tokyo]);
            mapCities[City.Shanghai].AddConnectedCities(mapCities[City.Beijing], mapCities[City.HongKong], mapCities[City.Seoul], mapCities[City.Taipei], mapCities[City.Tokyo]);
            mapCities[City.StPetersburg].AddConnectedCities(mapCities[City.Essen], mapCities[City.Istanbul], mapCities[City.Moscow]);
            mapCities[City.Sydney].AddConnectedCities(mapCities[City.Jakarta], mapCities[City.LosAngeles], mapCities[City.Manila]);
            mapCities[City.Taipei].AddConnectedCities(mapCities[City.HongKong], mapCities[City.Manila], mapCities[City.Osaka], mapCities[City.Shanghai]);
            mapCities[City.Tehran].AddConnectedCities(mapCities[City.Baghdad], mapCities[City.Delhi], mapCities[City.Karachi], mapCities[City.Moscow]);
            mapCities[City.Tokyo].AddConnectedCities(mapCities[City.Osaka], mapCities[City.SanFrancisco], mapCities[City.Seoul], mapCities[City.Shanghai]);
            mapCities[City.Washington].AddConnectedCities(mapCities[City.Atlanta], mapCities[City.Miami], mapCities[City.Montreal], mapCities[City.NewYork]);

            return new WorldMap(mapCities);
        }

        public IList<City> GetCities()
        {
            return new List<City>(48)
            {
                new City(City.Algiers, DiseaseColor.Black),
                new City(City.Atlanta, DiseaseColor.Blue),
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

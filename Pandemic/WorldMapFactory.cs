using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pandemic.City;

namespace Pandemic
{
    public class WorldMapFactory
    {       
        public WorldMapFactory(IDictionary<DiseaseColor, Disease> diseases)
        {
            Diseases = diseases ?? throw new ArgumentNullException(nameof(diseases));
        }

        public IDictionary<DiseaseColor, Disease> Diseases { get; set; }

        public WorldMap BuildMap()
        {
            var mapCities = new Dictionary<string, MapCity>(GetCities().ToDictionary(x => x.Name, x => new MapCity(x, Diseases)));

            mapCities[Algiers].AddConnectedCities(mapCities[Cairo], mapCities[Istanbul], mapCities[Madrid], mapCities[Paris]);
            mapCities[Atlanta].AddConnectedCities(mapCities[Chicago], mapCities[Miami], mapCities[Washington]);
            mapCities[Baghdad].AddConnectedCities(mapCities[Cairo], mapCities[Istanbul], mapCities[Tehran], mapCities[Riyadh]);
            mapCities[Bangkok].AddConnectedCities(mapCities[Jakarta], mapCities[HoChiMinhCity], mapCities[HongKong], mapCities[Kolkata]);
            mapCities[Beijing].AddConnectedCities(mapCities[Shanghai], mapCities[Seoul]);
            mapCities[Bogota].AddConnectedCities(mapCities[BuenosAires], mapCities[MexicoCity], mapCities[Miami], mapCities[Lima], mapCities[SaoPaulo]);
            mapCities[BuenosAires].AddConnectedCities(mapCities[Bogota], mapCities[SaoPaulo]);
            mapCities[Cairo].AddConnectedCities(mapCities[Algiers], mapCities[Baghdad], mapCities[Istanbul], mapCities[Khartoum], mapCities[Riyadh]);
            mapCities[Chennai].AddConnectedCities(mapCities[Bangkok], mapCities[Delhi], mapCities[Jakarta], mapCities[Kolkata], mapCities[Mumbai]);
            mapCities[Chicago].AddConnectedCities(mapCities[Atlanta], mapCities[LosAngeles], mapCities[MexicoCity], mapCities[Montreal], mapCities[SanFrancisco]);
            mapCities[Delhi].AddConnectedCities(mapCities[Chennai], mapCities[Karachi], mapCities[Kolkata], mapCities[Mumbai], mapCities[Tehran]);
            mapCities[Essen].AddConnectedCities(mapCities[London], mapCities[Milan], mapCities[Paris], mapCities[StPetersburg]);
            mapCities[HoChiMinhCity].AddConnectedCities(mapCities[Bangkok], mapCities[Jakarta], mapCities[HongKong], mapCities[Manila]);
            mapCities[HongKong].AddConnectedCities(mapCities[Bangkok], mapCities[HoChiMinhCity], mapCities[Kolkata], mapCities[Manila], mapCities[Shanghai], mapCities[Taipei]);
            mapCities[Istanbul].AddConnectedCities(mapCities[Algiers], mapCities[Baghdad], mapCities[Cairo], mapCities[Milan], mapCities[Moscow]);
            mapCities[Jakarta].AddConnectedCities(mapCities[Bangkok], mapCities[Chennai], mapCities[HoChiMinhCity], mapCities[Sydney]);
            mapCities[Johannesburg].AddConnectedCities(mapCities[BuenosAires], mapCities[Kinshasa], mapCities[Khartoum]);
            mapCities[Karachi].AddConnectedCities(mapCities[Delhi], mapCities[Mumbai], mapCities[Riyadh], mapCities[Tehran]);
            mapCities[Khartoum].AddConnectedCities(mapCities[Cairo], mapCities[Johannesburg], mapCities[Kinshasa], mapCities[Lagos]);
            mapCities[Kinshasa].AddConnectedCities(mapCities[Johannesburg], mapCities[Lagos], mapCities[Khartoum]);
            mapCities[Kolkata].AddConnectedCities(mapCities[Bangkok], mapCities[Chennai], mapCities[Delhi], mapCities[HongKong]);
            mapCities[Lagos].AddConnectedCities(mapCities[Kinshasa], mapCities[Khartoum], mapCities[SaoPaulo]);
            mapCities[Lima].AddConnectedCities(mapCities[Bogota], mapCities[MexicoCity], mapCities[LosAngeles], mapCities[Santiago]);
            mapCities[London].AddConnectedCities(mapCities[Essen], mapCities[Madrid], mapCities[NewYork], mapCities[Paris]);
            mapCities[LosAngeles].AddConnectedCities(mapCities[Chicago], mapCities[MexicoCity], mapCities[SanFrancisco], mapCities[Sydney]);
            mapCities[Madrid].AddConnectedCities(mapCities[Algiers], mapCities[London], mapCities[NewYork], mapCities[Madrid], mapCities[SaoPaulo]);
            mapCities[Manila].AddConnectedCities(mapCities[HoChiMinhCity], mapCities[HongKong], mapCities[SanFrancisco], mapCities[Sydney], mapCities[Taipei]);
            mapCities[MexicoCity].AddConnectedCities(mapCities[Bogota], mapCities[Chicago], mapCities[Lima], mapCities[LosAngeles], mapCities[Miami]);
            mapCities[Miami].AddConnectedCities(mapCities[Atlanta], mapCities[Bogota], mapCities[MexicoCity], mapCities[Washington]);
            mapCities[Milan].AddConnectedCities(mapCities[Essen], mapCities[Istanbul], mapCities[Paris]);
            mapCities[Montreal].AddConnectedCities(mapCities[Chicago], mapCities[NewYork], mapCities[Washington]);
            mapCities[Moscow].AddConnectedCities(mapCities[Istanbul], mapCities[StPetersburg], mapCities[Tehran]);
            mapCities[Mumbai].AddConnectedCities(mapCities[Chennai], mapCities[Delhi], mapCities[Karachi]);
            mapCities[NewYork].AddConnectedCities(mapCities[London], mapCities[Madrid], mapCities[Montreal], mapCities[Washington]);
            mapCities[Osaka].AddConnectedCities(mapCities[Taipei], mapCities[Tokyo]);
            mapCities[Paris].AddConnectedCities(mapCities[Algiers], mapCities[Essen], mapCities[London], mapCities[Madrid], mapCities[Milan]);
            mapCities[Riyadh].AddConnectedCities(mapCities[Baghdad], mapCities[Cairo], mapCities[Karachi]);
            mapCities[SanFrancisco].AddConnectedCities(mapCities[Chicago], mapCities[LosAngeles], mapCities[Manila], mapCities[Tokyo]);
            mapCities[Santiago].AddConnectedCities(mapCities[BuenosAires], mapCities[Lima]);
            mapCities[SaoPaulo].AddConnectedCities(mapCities[Bogota], mapCities[BuenosAires], mapCities[Lagos], mapCities[Madrid]);
            mapCities[Seoul].AddConnectedCities(mapCities[Beijing], mapCities[Shanghai], mapCities[Tokyo]);
            mapCities[Shanghai].AddConnectedCities(mapCities[Beijing], mapCities[HongKong], mapCities[Seoul], mapCities[Taipei], mapCities[Tokyo]);
            mapCities[StPetersburg].AddConnectedCities(mapCities[Essen], mapCities[Istanbul], mapCities[Moscow]);
            mapCities[Sydney].AddConnectedCities(mapCities[Jakarta], mapCities[LosAngeles], mapCities[Manila]);
            mapCities[Taipei].AddConnectedCities(mapCities[HongKong], mapCities[Manila], mapCities[Osaka], mapCities[Shanghai]);
            mapCities[Tehran].AddConnectedCities(mapCities[Baghdad], mapCities[Delhi], mapCities[Karachi], mapCities[Moscow]);
            mapCities[Tokyo].AddConnectedCities(mapCities[Osaka], mapCities[SanFrancisco], mapCities[Seoul], mapCities[Shanghai]);
            mapCities[Washington].AddConnectedCities(mapCities[Atlanta], mapCities[Miami], mapCities[Montreal], mapCities[NewYork]);

            return new WorldMap(mapCities);
        }

        public IList<City> GetCities()
        {
            return new List<City>(48)
            {
                new City(Algiers, DiseaseColor.Black),
                new City(Atlanta, DiseaseColor.Blue),
                new City(Baghdad, DiseaseColor.Black),
                new City(Bangkok, DiseaseColor.Red),
                new City(Beijing, DiseaseColor.Red),
                new City(Bogota, DiseaseColor.Yellow),
                new City(BuenosAires, DiseaseColor.Yellow),
                new City(Cairo, DiseaseColor.Black),
                new City(Chennai, DiseaseColor.Black),
                new City(Chicago, DiseaseColor.Blue),
                new City(Delhi, DiseaseColor.Black),
                new City(Essen, DiseaseColor.Blue),
                new City(HoChiMinhCity, DiseaseColor.Red),
                new City(HongKong, DiseaseColor.Red),
                new City(Istanbul, DiseaseColor.Black),
                new City(Jakarta, DiseaseColor.Red),
                new City(Johannesburg, DiseaseColor.Yellow),
                new City(Karachi, DiseaseColor.Black),
                new City(Khartoum, DiseaseColor.Yellow),
                new City(Kinshasa, DiseaseColor.Yellow),
                new City(Kolkata, DiseaseColor.Black),
                new City(Lagos, DiseaseColor.Yellow),
                new City(Lima, DiseaseColor.Yellow),
                new City(London, DiseaseColor.Blue),
                new City(LosAngeles, DiseaseColor.Yellow),
                new City(Madrid, DiseaseColor.Blue),
                new City(Manila, DiseaseColor.Red),
                new City(MexicoCity, DiseaseColor.Yellow),
                new City(Miami, DiseaseColor.Yellow),
                new City(Milan, DiseaseColor.Blue),
                new City(Montreal, DiseaseColor.Blue),
                new City(Moscow, DiseaseColor.Black),
                new City(Mumbai, DiseaseColor.Black),
                new City(NewYork, DiseaseColor.Blue),
                new City(Osaka, DiseaseColor.Red),
                new City(Paris, DiseaseColor.Blue),
                new City(Riyadh, DiseaseColor.Black),
                new City(SanFrancisco, DiseaseColor.Blue),
                new City(Santiago, DiseaseColor.Yellow),
                new City(SaoPaulo, DiseaseColor.Yellow),
                new City(Seoul, DiseaseColor.Red),
                new City(Shanghai, DiseaseColor.Red),
                new City(StPetersburg, DiseaseColor.Blue),
                new City(Sydney, DiseaseColor.Red),
                new City(Taipei, DiseaseColor.Red),
                new City(Tehran, DiseaseColor.Black),
                new City(Tokyo, DiseaseColor.Red),
                new City(Washington, DiseaseColor.Blue)
            };
        }
    }
}

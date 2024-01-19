using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.Helpers;

namespace Game.Pandemic.ViewModels
{
    public class WorldMapViewModel : ViewModelBase
    {
        public WorldMapViewModel(WorldMap worldMap)
        {
            var coordinates = CityCoordinates.Get();
            WorldMap = new KeyExtractorDictionary<string, CityViewModel>(x => x.Coordinates.CityName, 
                worldMap.Cities.Select(c => new CityViewModel(c, coordinates[c.City.Name])));
        }

        public IDictionary<string, CityViewModel> WorldMap { get; }

        public Size Size { get; } = new Size(1400, 700);
    }

    public class CityViewModel
    {
        public CityViewModel(MapCity mapCity, CityCoordinates coordinates)
        {
            MapCity = mapCity ?? throw new ArgumentNullException(nameof(mapCity));
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        }

        public MapCity MapCity { get; }
        public CityCoordinates Coordinates { get; }
    }

    public class CityCoordinates
    {
        public CityCoordinates(string city, int x, int y)
        {
            CityName = city;
            X = x;
            Y = y;
        }

        public string CityName { get; }
        public int X { get; }
        public int Y { get; }

        public static IDictionary<string, CityCoordinates> Get()
        {
            return new KeyExtractorDictionary<string, CityCoordinates>(x => x.CityName)
            {
                new CityCoordinates(City.SanFrancisco, 166, 253),
                new CityCoordinates(City.LosAngeles, 166, 325),
                new CityCoordinates(City.MexicoCity, 251, 345),
                new CityCoordinates(City.Chicago, 258, 223),
                new CityCoordinates(City.Atlanta, 294, 278),
                new CityCoordinates(City.Lima, 300, 487),
                new CityCoordinates(City.Santiago, 319, 566),
                new CityCoordinates(City.Bogota, 342, 410),
                new CityCoordinates(City.Montreal, 346, 217),
                new CityCoordinates(City.Miami, 353, 332),
                new CityCoordinates(City.Washington, 397, 270),
                new CityCoordinates(City.BuenosAires, 410, 547),
                new CityCoordinates(City.NewYork, 420, 220),
                new CityCoordinates(City.SaoPaulo, 461, 488),
                new CityCoordinates(City.Madrid, 560, 259),
                new CityCoordinates(City.London, 570, 182),
                new CityCoordinates(City.Lagos, 624, 390),
                new CityCoordinates(City.Paris, 640, 223),
                new CityCoordinates(City.Algiers, 652, 302),
                new CityCoordinates(City.Essen, 662, 170),
                new CityCoordinates(City.Kinshasa, 683, 445),
                new CityCoordinates(City.Milan, 701, 213),
                new CityCoordinates(City.Cairo, 719, 313),
                new CityCoordinates(City.Johannesburg, 725, 525),
                new CityCoordinates(City.Istanbul, 731, 247),
                new CityCoordinates(City.Khartoum, 734, 374),
                new CityCoordinates(City.StPetersburg, 756, 148),
                new CityCoordinates(City.Baghdad, 791, 275),
                new CityCoordinates(City.Moscow, 794, 200),
                new CityCoordinates(City.Riyadh, 802, 345),
                new CityCoordinates(City.Tehran, 853, 237),
                new CityCoordinates(City.Karachi, 872, 303),
                new CityCoordinates(City.Mumbai, 880, 366),
                new CityCoordinates(City.Delhi, 932, 274),
                new CityCoordinates(City.Chennai, 951, 397),
                new CityCoordinates(City.Kolkata, 989, 299),
                new CityCoordinates(City.Jakarta, 1004, 467),
                new CityCoordinates(City.Bangkok, 1013, 364),
                new CityCoordinates(City.Beijing, 1049, 203),
                new CityCoordinates(City.Shanghai, 1050, 259),
                new CityCoordinates(City.HongKong, 1054, 325),
                new CityCoordinates(City.HoChiMinhCity, 1056, 411),
                new CityCoordinates(City.Seoul, 1120, 199),
                new CityCoordinates(City.Taipei, 1125, 319),
                new CityCoordinates(City.Manila, 1143, 406),
                new CityCoordinates(City.Tokyo, 1188, 233),
                new CityCoordinates(City.Sydney, 1188, 546),
                new CityCoordinates(City.Osaka, 1198, 296)
            };
        }
    }
}
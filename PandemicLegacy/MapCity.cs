using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PandemicLegacy.Common;

namespace PandemicLegacy
{
    public class MapCity
    {
        public City City { get; private set; }

        public bool HasReaserchStation { get; set; }
        private IEnumerable<MapCity> ConnectedCities { get; set; }
        private IDictionary<DiseaseColor, int> Infections { get; set; }
        public int Population { get; private set; }
        public double Area { get; private set; }

        public MapCity(City city)
        {
            this.City = city;

            this.Infections = new Dictionary<DiseaseColor, int>(4)
            {
                {DiseaseColor.Black, 0 },
                {DiseaseColor.Blue, 0 },
                {DiseaseColor.Red, 0 },
                {DiseaseColor.Yellow, 0 }
            };
        }

        public bool IsCityConnected(MapCity toCity)
        {
            return ConnectedCities.Contains(toCity);
        }

        public void ChangeInfection(DiseaseColor color, int value)
        {
            if (value < 0)
                this.Infections[color] = 0;
            else if (value > 3)
                this.Infections[color] = 3;
            else
                this.Infections[color] = value;
        }

        public void RemoveInfection(DiseaseColor color)
        {
            this.Infections[color] = 0;
        }

        public void AddConnectedCities(params MapCity[] cities)
        {
            this.ConnectedCities = cities.ToList();
        }

        public override string ToString()
        {
            return City.ToString();
        }
    }
}

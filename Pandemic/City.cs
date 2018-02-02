using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class City : IEquatable<City>
    {
        public string Name { get; private set; }
        public DiseaseColor Color { get; private set; }

        public City(string name, DiseaseColor color)
        {
            Name = name;
            Color = color;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Name, this.Color);
        }

        public override bool Equals(object other)
        {
            // other could be a reference type, the is operator will return false if null
            if (other is City)
                return Equals((City)other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() * 17 ^ this.Color.GetHashCode();
        }

        public bool Equals(City other)
        {
            return this.Name == other.Name && this.Color == other.Color;
        }

        public static bool operator ==(City c1, City c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(City c1, City c2)
        {
            return !c1.Equals(c2);
        }


        public const string Algiers = "Algiers";
        public const string Atlanta = "Atlanta";
        public const string Baghdad = "Baghdad";
        public const string Bangkok = "Bangkok";
        public const string Beijing = "Beijing";
        public const string Bogota = "Bogota";
        public const string BuenosAires = "Buenos Aires";
        public const string Cairo = "Cairo";
        public const string Chennai = "Chennai";
        public const string Chicago = "Chicago";
        public const string Delhi = "Delhi";
        public const string Essen = "Essen";
        public const string HoChiMinhCity = "Ho Chi Minh City";
        public const string HongKong = "Hong Kong";
        public const string Istanbul = "Istanbul";
        public const string Jakarta = "Jakarta";
        public const string Johannesburg = "Johannesburg";
        public const string Karachi = "Karachi";
        public const string Khartoum = "Khartoum";
        public const string Kinshasa = "Kinshasa";
        public const string Kolkata = "Kolkata";
        public const string Lagos = "Lagos";
        public const string Lima = "Lima";
        public const string London = "London";
        public const string LosAngeles = "Los Angeles";
        public const string Madrid = "Madrid";
        public const string Manila = "Manila";
        public const string MexicoCity = "Mexico City";
        public const string Miami = "Miami";
        public const string Milan = "Milan";
        public const string Montreal = "Montreal";
        public const string Moscow = "Moscow";
        public const string Mumbai = "Mumbai";
        public const string NewYork = "New York";
        public const string Osaka = "Osaka";
        public const string Paris = "Paris";
        public const string Riyadh = "Riyadh";
        public const string SanFrancisco = "San Francisco";
        public const string Santiago = "Santiago";
        public const string SaoPaulo = "Sao Paulo";
        public const string Seoul = "Seoul";
        public const string Shanghai = "Shanghai";
        public const string StPetersburg = "St. Petersburg";
        public const string Sydney = "Sydney";
        public const string Taipei = "Taipei";
        public const string Tehran = "Tehran";
        public const string Tokyo = "Tokyo";
        public const string Washington = "Washington";
    }
}

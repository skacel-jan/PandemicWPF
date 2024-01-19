using System;

namespace Game.Pandemic.GameLogic.Board
{
    public class City : IEquatable<City>
    {
        public City(string name, DiseaseColor color)
        {
            Name = name;
            Color = color;
        }

        public DiseaseColor Color { get; private set; }
        public string Name { get; private set; }

        public int CompareTo(City other)
        {
            int result = Color.CompareTo(other.Color);
            if (result == 0)
            {
                result = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            }
            return result;
        }

        public override bool Equals(object other)
        {
            // other could be a reference type, the is operator will return false if null
            if (other is City city)
                return Equals(city);
            else
                return false;
        }

        public bool Equals(City other)
        {
            return Name == other.Name && Color == other.Color;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17 ^ Color.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} - {Color}";
        }

        public static int Compare(City left, City right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }
            if (left is null)
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        #region Cities strings

        public const string Algiers = "Algiers";
        public const string Atlanta = "Atlanta";
        public const string Baghdad = "Baghdad";
        public const string Bangkok = "Bangkok";
        public const string Beijing = "Beijing";
        public const string Bogota = "Bogota";
        public const string BuenosAires = "Buenos Aires";
        public const string Cairo = "Cairo";
        public const string Delhi = "Delhi";
        public const string Essen = "Essen";
        public const string HoChiMinhCity = "Ho Chi Minh City";
        public const string HongKong = "Hong Kong";
        public const string Chennai = "Chennai";
        public const string Chicago = "Chicago";
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

        #endregion Cities strings
    }
}
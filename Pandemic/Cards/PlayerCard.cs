using System;

namespace Pandemic.Cards
{
    public class PlayerCard : CityCard, IComparable
    {
        public PlayerCard(City city) : base(city)
        {
        }
    }
}
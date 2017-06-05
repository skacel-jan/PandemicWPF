using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy.Decks
{
    public class PlayerDeck : Deck<Card>
    {
        public PlayerDeck(IEnumerable<City> cities) : this(cities.Select(city => new PlayerCard(city)))
        { }

        public PlayerDeck(IEnumerable<Card> cards) : base(cards)
        { }

        public void AddEpidemicCards(int epidemicCount)
        {
            this.AddRange(Enumerable.Repeat(new EpidemicCard(), epidemicCount));
        }
    }
}

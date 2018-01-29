using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Decks
{
    public class PlayerDeck : Deck<Card>
    {
        public PlayerDeck(IEnumerable<City> cities) : this(cities.Select(city => new PlayerCard(city)))
        { }

        public PlayerDeck(IEnumerable<Card> cards) : base(cards)
        { }

        public void AddEpidemicCards(int epidemicCount)
        {
            foreach (var card in Enumerable.Repeat(new EpidemicCard(), epidemicCount))
            {
                this.Add(card);
            }

        }
    }
}

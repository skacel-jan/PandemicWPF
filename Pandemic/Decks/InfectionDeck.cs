using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Decks
{
    public class InfectionDeck : Deck<InfectionCard>
    {
        public InfectionDeck(IEnumerable<City> cities) : this(cities.Select(city => new InfectionCard(city)))
        {
        }

        public InfectionDeck(IEnumerable<InfectionCard> cards) : base(cards)
        {
        }
    }
}

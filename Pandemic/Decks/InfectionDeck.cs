using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Decks
{
    public class InfectionDeck : Deck<InfectionCard>
    {
        public InfectionDeck() : base()
        {
        }

        public InfectionDeck(IEnumerable<City> cities) : this(cities.Select(city => new InfectionCard(city)))
        {
        }

        public InfectionDeck(IEnumerable<InfectionCard> cards) : base(cards)
        {
        }
    }

    public class InfectionDeckFactory : IInfectionDeckFactory
    {
        public IDeck<InfectionCard> GetInfectionDeck(IEnumerable<City> cities)
        {
            return new InfectionDeck(cities);
        }
        public IDeck<InfectionCard> GetInfectionDeck(IEnumerable<InfectionCard> cards)
        {
            return new InfectionDeck(cards);
        }

        public IDeck<InfectionCard> GetEmptyInfectionDeck()
        {
            return new InfectionDeck();
        }
    }
}

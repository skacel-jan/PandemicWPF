using System.Collections.Generic;

namespace Pandemic.Decks
{
    public interface IInfectionDeckFactory
    {
        Deck<InfectionCard> GetEmptyInfectionDeck();
        Deck<InfectionCard> GetInfectionDeck(IEnumerable<City> cities);
        Deck<InfectionCard> GetInfectionDeck(IEnumerable<InfectionCard> cards);
    }
}
using System.Collections.Generic;

namespace Pandemic.Decks
{
    public interface IInfectionDeckFactory
    {
        IDeck<InfectionCard> GetEmptyInfectionDeck();
        IDeck<InfectionCard> GetInfectionDeck(IEnumerable<City> cities);
        IDeck<InfectionCard> GetInfectionDeck(IEnumerable<InfectionCard> cards);
    }
}
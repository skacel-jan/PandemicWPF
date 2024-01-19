using Game.Pandemic.GameLogic.Board;

namespace Game.Pandemic.GameLogic.Cards
{
    public class InfectionCard : Card
    {
        public InfectionCard(City city) : base($"Infection - {city.Name}")
        {
            City = city;
        }

        public City City { get; }

        public override int SortCode => 3;
    }
}
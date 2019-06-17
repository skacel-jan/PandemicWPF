namespace Pandemic.Cards
{
    public class InfectionCard : Card
    {
        public InfectionCard(City city) : base($"Infection - {city.Name}")
        {
            City = city;
        }

        public City City { get; }

        public override int SortRank => 3;
    }
}
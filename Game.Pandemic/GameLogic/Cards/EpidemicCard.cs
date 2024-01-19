namespace Game.Pandemic.GameLogic.Cards
{
    public class EpidemicCard : PlayerCard
    {
        public override int SortCode => 4;

        public EpidemicCard() : base("Epidemic")
        {
        }
    }
}
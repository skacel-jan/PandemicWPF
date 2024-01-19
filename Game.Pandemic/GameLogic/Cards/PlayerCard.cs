using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Cards
{
    public abstract class PlayerCard : Card
    {
        protected PlayerCard(string name) : base(name)
        {
        }
        public Character Character { get; set; }
    }
}
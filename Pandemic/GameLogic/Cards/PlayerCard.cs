using System;

namespace Pandemic.Cards
{
    public abstract class PlayerCard : Card
    {
        protected PlayerCard(string name) : base(name)
        {
        }
        public Character Character { get; set; }
    }
}
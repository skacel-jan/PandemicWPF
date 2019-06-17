using System.Collections.Generic;
using System.Linq;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class OneQuietNightAction : EventAction
    {
        public OneQuietNightAction(EventCard card, Game game) : base(card, game)
        {
        }

        protected override void AddEffects()
        {
            base.AddEffects();
            Effects.Add(new OneQuietNightEffect(Game.Infection));
        }

        protected override void Initialize()
        {
        }
    }
}
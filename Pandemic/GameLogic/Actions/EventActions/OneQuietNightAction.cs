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
            Effects.Add(new OneQuietNightEffect(Game.Infection));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            return Enumerable.Empty<Selection>();
        }
    }
}
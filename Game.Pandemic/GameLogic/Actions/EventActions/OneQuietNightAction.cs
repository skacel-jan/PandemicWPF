using Game.Pandemic.GameLogic.Actions.Effects;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Actions.EventActions
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
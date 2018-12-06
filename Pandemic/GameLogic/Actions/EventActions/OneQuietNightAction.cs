using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class OneQuietNightAction : EventAction
    {
        public OneQuietNightAction(EventCard card, Game game) : base(card, game)
        {
        }

        public override void Execute()
        {
            Game.Infection.Actual = 0;

            FinishAction();
        }
    }
}
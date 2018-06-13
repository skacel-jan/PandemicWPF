using Pandemic.GameLogic;
using System;

namespace Pandemic.Cards
{
    public class OneQuietNightCard : EventCard
    {
        public OneQuietNightCard() : base("One Quiet Night")
        {
        }

        public override void PlayEvent(Game game)
        {
            game.Infection.Actual = 0;

            OnEventFinished(EventArgs.Empty, game);
        }
    }
}
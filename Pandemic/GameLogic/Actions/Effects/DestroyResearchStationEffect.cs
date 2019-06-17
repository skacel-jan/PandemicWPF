using System;

namespace Pandemic.GameLogic.Actions
{
    public class DestroyResearchStationEffect : IEffect
    {
        private readonly MapCity _city;
        private readonly Game _game;

        public DestroyResearchStationEffect(MapCity city, Game game)
        {
            _city = city ?? throw new ArgumentNullException(nameof(city));
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Execute()
        {
            _city.HasResearchStation = false;
            _game.ResearchStationsPile++;

        }
    }
}
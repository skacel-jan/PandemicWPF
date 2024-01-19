using System;
using Game.Pandemic.GameLogic.Board;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    public class BuildResearchStationEffect : IEffect
    {
        private readonly MapCity _city;
        private readonly Game _game;

        public BuildResearchStationEffect(MapCity city, Game game)
        {
            _city = city ?? throw new ArgumentNullException(nameof(city));
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Execute()
        {
            _city.HasResearchStation = true;
            _game.ResearchStationsPile--;
        }
    }
}
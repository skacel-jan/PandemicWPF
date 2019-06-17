﻿namespace Pandemic.GameLogic.Actions
{
    public class UnselectAllCitiesEffect : IEffect
    {
        private readonly WorldMap _worldMap;

        public UnselectAllCitiesEffect(WorldMap worldMap)
        {
            _worldMap = worldMap;
        }

        public void Execute()
        {
            foreach (var mapCity in _worldMap.Cities)
            {
                mapCity.IsSelectable = false;
            }
        }
    }
}
using System.Collections.Generic;

namespace Game.Pandemic.GameLogic.Board
{
    public interface IWorldMapFactory
    {
        WorldMap CreateWorldMap(IDictionary<DiseaseColor, Disease> diseases);
    }
}
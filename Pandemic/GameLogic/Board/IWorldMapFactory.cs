using Pandemic.GameLogic;
using System.Collections.Generic;

namespace Pandemic
{
    public interface IWorldMapFactory
    {
        WorldMap CreateWorldMap(IDictionary<DiseaseColor, Disease> diseases);
    }
}
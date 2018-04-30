using System;

namespace Pandemic
{
    public class BuildBehaviour
    {
        public BuildBehaviour(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }

        public Character Character { get; }

        public virtual void Build(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
            Character.RemoveCard(mapCity.City);
        }

        public virtual bool CanBuild(MapCity mapCity)
        {
            return !mapCity.HasResearchStation && Character.HasCityCard(mapCity.City);
        }

        public virtual bool IsPossible()
        {
            return CanBuild(Character.CurrentMapCity);
        }
    }

    public class OperationsExpertBuildBehaviour : BuildBehaviour
    {
        public OperationsExpertBuildBehaviour(Character character) : base(character)
        {
        }

        public override void Build(MapCity mapCity)
        {
            mapCity.HasResearchStation = true;
        }

        public override bool CanBuild(MapCity mapCity)
        {
            return !mapCity.HasResearchStation;
        }
    }
}
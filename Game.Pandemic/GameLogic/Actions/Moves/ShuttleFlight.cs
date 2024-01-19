using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public class ShuttleFlight : IMoveAction
    {
        public ShuttleFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; }
        public string MoveType => ActionTypes.ShuttleFlight;

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && city.HasResearchStation;
        }

        public override string ToString() => MoveType;
    }
}
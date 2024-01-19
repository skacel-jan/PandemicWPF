using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public class DriveOrFerry : IMoveAction
    {
        public DriveOrFerry(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }

        public string MoveType { get => ActionTypes.DriveOrFerry; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.IsCityConnected(city);
        }

        public override string ToString() => MoveType;
    }
}
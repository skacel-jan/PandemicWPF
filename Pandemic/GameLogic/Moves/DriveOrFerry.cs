using System;

namespace Pandemic.GameLogic.Actions
{
    public class DriveOrFerry : IMoveAction
    {
        public DriveOrFerry(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }

        public bool IsCardRequired => false;
        public string MoveType { get => ActionTypes.DriveOrFerry; }

        public bool IsPossible(Game game, MapCity city)
        {
            return Character.CurrentMapCity.IsCityConnected(city);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            Character.CurrentMapCity = city;
            moveActionCallback();
        }

        public override string ToString() => MoveType;
    }
}
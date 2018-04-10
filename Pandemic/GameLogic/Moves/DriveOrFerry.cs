namespace Pandemic
{
    public class DriveOrFerry : IMoveAction
    {
        public DriveOrFerry(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.DriveOrFerry; }

        public bool IsCardNeeded { get => false; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.IsCityConnected(city);
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            return true;
        }
    }
}
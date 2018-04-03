namespace Pandemic
{
    public class ShuttleFlight : IMoveAction
    {
        public string MoveType { get => ActionTypes.ShuttleFlight; }

        public Character Character { get; set; }

        public bool IsCardNeeded => false;

        public ShuttleFlight(Character character)
        {
            Character = character;
        }

        public bool CanMove(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && city.HasResearchStation;
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            return true;
        }
    }
}
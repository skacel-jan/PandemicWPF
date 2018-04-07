namespace Pandemic
{
    public class OperationsExpertSpecialMove : IMoveCardAction
    {
        public OperationsExpertSpecialMove(Character character)
        {
            Character = character;
        }

        public PlayerCard Card { get; set; }
        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.OperationsExpertSpecialMove; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation;
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Card);
            return true;
        }
    }
}
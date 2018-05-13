using Pandemic.Cards;

namespace Pandemic
{
    public class OperationsExpertSpecialMove : IMoveCardAction
    {
        public OperationsExpertSpecialMove(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType { get => ActionTypes.OperationsExpertSpecialMove; }

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.Cards.Count > 0;
        }

        public bool Move(MapCity city, PlayerCard card)
        {            
            Character.CurrentMapCity = city;
            Character.RemoveCard(card);
            return true;
        }
    }
}
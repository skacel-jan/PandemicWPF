using System.Linq;
using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public class OperationsExpertSpecialMove : ICardMoveAction
    {
        public OperationsExpertSpecialMove(OperationsExpert character)
        {
            Character = character;
        }

        public Character Character { get; }
        public string MoveType => "Special move";

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.Cards.OfType<CityCard>().Any();
        }        

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card) => true;
    }
}
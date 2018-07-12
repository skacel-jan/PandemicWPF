using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class OperationsExpertSpecialMove : IMoveAction
    {
        public OperationsExpertSpecialMove(OperationsExpert character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => true;
        public string MoveType => "Special move";

        public bool IsPossible(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && Character.Cards.Count > 0;
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            game.SelectCard(Character.Cards, (Card card) =>
            {
                game.MoveCharacter(Character, city);
                Character.RemoveCard(card);
                game.AddCardToPlayerDiscardPile(card);
                moveActionCallback();

            }, "Select card of a current city");
        }
    }
}
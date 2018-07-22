using Pandemic.Cards;
using Pandemic.Characters;
using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class OperationsExpertSpecialMove : IMoveAction
    {
        private int _lastTurnExecuted = 0;

        public OperationsExpertSpecialMove(OperationsExpert character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => true;
        public string MoveType => "Special move";

        public bool IsPossible(Game game, MapCity city)
        {
            return game.Turn != _lastTurnExecuted && Character.CurrentMapCity.HasResearchStation && Character.Cards.Count > 0;
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            game.SelectCard(Character.Cards, (Card card) =>
            {
                game.MoveCharacter(Character, city);
                Character.RemoveCard(card);
                game.AddCardToPlayerDiscardPile(card);
                _lastTurnExecuted = game.Turn;
                moveActionCallback();
                return true;

            }, "Select any card");
        }

        public override string ToString()
        {
            return MoveType;
        }
    }
}
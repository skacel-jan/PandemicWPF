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
            var action = new SelectAction<PlayerCard>(SetCard, Character.Cards,
                $"Select any card");

            game.SelectionService.Select(action);

            void SetCard(PlayerCard selectedCard)
            {
                Character.CurrentMapCity = city;
                Character.RemoveCard(selectedCard);
                game.AddCardToPlayerDiscardPile(selectedCard);
                _lastTurnExecuted = game.Turn;
                moveActionCallback();
            }
        }

        public override string ToString() => MoveType;
    }
}
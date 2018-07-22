using System;
using System.Linq;
using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
{
    public class CharterFlight : IMoveAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.CharterFlight;

        public bool IsCardRequired => true;

        public bool IsPossible(Game game, MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            game.SelectCard(Character.Cards.OfType<CityCard>(), (Card card) =>
            {
                if (card is CityCard cityCard)
                {
                    if (Character.CurrentMapCity.City == cityCard.City)
                    {
                        game.MoveCharacter(Character, city);
                        Character.RemoveCard(card);
                        game.AddCardToPlayerDiscardPile(card);
                        moveActionCallback();

                        return true;
                    }
                }
                return false;
            }, "Select card of a current city");
        }

        public override string ToString()
        {
            return MoveType;
        }
    }
}
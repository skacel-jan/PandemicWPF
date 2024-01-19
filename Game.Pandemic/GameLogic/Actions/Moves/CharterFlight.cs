using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public class CharterFlight : ICardMoveAction
    {
        public CharterFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.CharterFlight;

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(Character.CurrentMapCity.City);
        }

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card)
        {
            return Character.CurrentMapCity.City == card.City;
        }
    }
}
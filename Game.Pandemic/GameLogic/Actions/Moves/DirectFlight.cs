using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;
using Game.Pandemic.GameLogic.Characters;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public class DirectFlight : ICardMoveAction
    {
        public DirectFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public string MoveType => ActionTypes.DirectFlight;

        public bool IsPossible(MapCity city)
        {
            return Character.HasCityCard(city.City);
        }

        public override string ToString() => MoveType;

        public bool Validate(MapCity city, CityCard card)
        {
            return city.City == card.City;
        }
    }
}
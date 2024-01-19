using Game.Pandemic.GameLogic.Board;
using Game.Pandemic.GameLogic.Cards;

namespace Game.Pandemic.GameLogic.Actions.Moves
{
    public interface IMoveAction
    {
        bool IsPossible(MapCity city);
    }

    public interface ICardMoveAction : IMoveAction
    {
        bool Validate(MapCity city, CityCard card);
    }
}
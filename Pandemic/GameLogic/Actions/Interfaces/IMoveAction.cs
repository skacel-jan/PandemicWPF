using Pandemic.Cards;

namespace Pandemic.GameLogic.Actions
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
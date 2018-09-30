using System;

namespace Pandemic.GameLogic.Actions
{
    public class ShuttleFlight : IMoveAction
    {
        public ShuttleFlight(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardRequired => false;
        public string MoveType => ActionTypes.ShuttleFlight;

        public bool IsPossible(Game game, MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation && city.HasResearchStation;
        }

        public void Move(Game game, MapCity city, Action moveActionCallback)
        {
            Character.CurrentMapCity = city;
            moveActionCallback();
        }

        public override string ToString() => MoveType;
    }
}
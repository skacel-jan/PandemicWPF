using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic
{
    public class OperationsExpertSpecialMove : IMoveAction
    {
        public OperationsExpertSpecialMove(Character character)
        {
            Character = character;
        }

        public Character Character { get; set; }
        public bool IsCardNeeded => true;
        public string MoveType { get => ActionTypes.OperationsExpertSpecialMove; }

        public bool CanMove(MapCity city)
        {
            return Character.CurrentMapCity.HasResearchStation;
        }

        public bool Move(MapCity city)
        {
            Character.CurrentMapCity = city;
            Character.RemoveCard(Character.SelectedCard);
            return true;
        }
    }
}

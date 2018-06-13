using Pandemic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.GameLogic.Actions
{
    public class EventAction : IGameAction
    {
        public bool CanExecute(Game game)
        {
            return game.EventCards.Count > 0;
        }

        public void Execute(Game game, Action callbackAction)
        {
        }        
    }
}

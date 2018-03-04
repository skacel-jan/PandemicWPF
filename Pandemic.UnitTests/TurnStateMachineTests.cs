using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic;
using Pandemic.Characters;
using Pandemic.UnitTests;

namespace PandemicLegacy.UnitTests
{
    [TestClass]
    public class TurnStateMachineTests
    {
        [TestMethod]
        public void TurnStateMachineTest()
        {
            var fsm = new TurnStateMachine(new Queue<Character>(
                new Character[] { new Medic() { CurrentMapCity = Helper.GetWorldMpa().GetCity("Atlanta") } }) );
            fsm.Start();
            fsm.DoAction(); // action
            fsm.DoAction(); // action
            fsm.DoAction(); // action
            fsm.DoAction(); // action
            fsm.DoAction(); // go to drawing phase
            fsm.DoAction(); // infect city
            fsm.DoAction(); // infect city
            fsm.DoAction(); // end turn
        }
    }
}

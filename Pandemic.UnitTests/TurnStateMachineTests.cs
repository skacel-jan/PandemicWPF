using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic;

namespace PandemicLegacy.UnitTests
{
    [TestClass]
    public class TurnStateMachineTests
    {
        [TestMethod]
        public void TurnStateMachineTest()
        {
            var fsm = new TurnStateMachine(null);
            fsm.Start();
            foreach (int i in Enumerable.Range(0, 50))
            {
                fsm.DoAction();
            }
            ;
        }
    }
}

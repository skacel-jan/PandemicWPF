using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pandemic.UnitTests
{
    [TestClass]
    public class WorldMapTests
    {
        [TestMethod]
        public void GenerateMapTest()
        {
            var factory = new WorldMapFactory(Disease.CreateDiseases());
            var map = factory.BuildMap();
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PandemicLegacy.UnitTests
{
    [TestClass]
    public class CityTests
    {
        [TestMethod]
        public void CreateCitiesTest()
        {
            var cities = Helper.GetCities();
        }
    }
}

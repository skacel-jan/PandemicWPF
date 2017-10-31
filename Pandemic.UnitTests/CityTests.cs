using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pandemic.UnitTests
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic;
using Pandemic.Characters;

namespace PandemicLegacy.UnitTests
{
    [TestClass]
    public class CharacterTests
    {
        [TestMethod]
        public void TestCircularCollection()
        {
            var col = new CircularCollection<int>(new int[] { 1, 2, 3, 4 });

            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
            System.Diagnostics.Debug.WriteLine(col.Next());
        }
    }
}

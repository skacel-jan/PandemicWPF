using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pandemic.UnitTests
{
    [TestClass]
    public class CityTests
    {
        [TestMethod]
        public void CreateCitiesTest()
        {
            //var cities = Helper.GetCities();
        }

        public List<string> GetPINs(string observed)
        {
            Dictionary<char, char[]> map = new Dictionary<char, char[]>() {
                {'1', new[] {'1', '2', '4'}},
                {'2', new[] {'1', '2', '3', '5'}},
                {'3', new[] {'2', '3', '6'}},
                {'4', new[] {'1', '4', '5', '7'}},
                {'5', new[] {'2', '4', '5', '6', '8'}},
                {'6', new[] {'3', '5', '6', '9'}},
                {'7', new[] {'4', '7', '8'}},
                {'8', new[] {'5', '7', '8', '9', '0'}},
                {'9', new[] {'6', '8', '9'}},
                {'0', new[] {'0', '8'}},
              };

            int width = observed.Aggregate(1, (a, c) => a * map[c].Length);
            char[,] table = new char[observed.Length, width];

            for (int i = 0; i < observed.Length; i++)
            {
                var numbers = map[observed[i]];
                for (int w = 0, r = 0; w < width; w += numbers.Length, r++)
                {
                    for (int j = w, k = 0; k < numbers.Length; j++, k++)
                    {
                        table[i, j] = numbers[(k + (r * i)) % numbers.Length];
                    }
                }

            }
            var result = new List<string>(width);
            foreach (int y in Enumerable.Range(0, table.GetLength(1)))
            {
                result.Add(Enumerable.Range(0, table.GetLength(0)).Select(x => table[x, y]).Aggregate(string.Empty, (a, c) => a + c));
            }

            return result;
        }
    }
}

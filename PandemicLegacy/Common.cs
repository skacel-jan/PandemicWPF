using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public static class Common
    {
        public enum DiseaseColor
        {
            Black,
            Blue,
            Red,
            Yellow
        }

        public static Dictionary<DiseaseColor, Disease> Diseases { get; private set; }

        static Common()
        {
            Diseases = new Dictionary<DiseaseColor, Disease>(4)
            {
                {DiseaseColor.Black, new Disease(DiseaseColor.Black)},
                {DiseaseColor.Blue, new Disease(DiseaseColor.Blue)},
                {DiseaseColor.Red, new Disease(DiseaseColor.Red)},
                {DiseaseColor.Yellow, new Disease(DiseaseColor.Yellow)}
            };
        }

    }
}

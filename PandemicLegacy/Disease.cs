using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy
{
    public enum DiseaseColor
    {
        Black,
        Blue,
        Red,
        Yellow
    }

    public class Disease
    {
        public DiseaseColor Color { get; private set; }

        public bool IsEradicated { get; set; }

        public bool KnownCure { get; set; }

        public Disease(DiseaseColor color)
        {
            this.Color = color;
        }

        public static Dictionary<DiseaseColor, Disease> Diseases { get; private set; }

        static Disease()
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

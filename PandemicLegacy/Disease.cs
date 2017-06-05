using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PandemicLegacy.Common;

namespace PandemicLegacy
{
    public class Disease
    {
        public DiseaseColor Color { get; private set; }

        public bool IsEradicated { get; set; }

        public bool KnownCure { get; set; }

        public Disease(DiseaseColor color)
        {
            this.Color = color;
        }
    }
}

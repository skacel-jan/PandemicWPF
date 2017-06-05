using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PandemicLegacy
{
    public class Pawn
    {
        public Color Color { get; private set; }
        public MapCity MapCity { get; set; }

        public Pawn(Color color)
        {
            this.Color = color;
        }
    }
}

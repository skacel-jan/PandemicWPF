using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PandemicLegacy
{
    public class Pawn : ObservableObject
    {
        public Color Color { get; private set; }

        private MapCity _mapCity;
        public MapCity MapCity
        {
            get { return _mapCity; }
            set { Set(ref _mapCity, value); }
        }

        private SolidColorBrush _colorBrush;
        public SolidColorBrush ColorBrush
        {
            get { return _colorBrush; }
            private set { Set(ref _colorBrush, value); }
        }

        public Pawn(Color color)
        {
            this.Color = color;
            this.ColorBrush = new SolidColorBrush(color);
        }
    }
}

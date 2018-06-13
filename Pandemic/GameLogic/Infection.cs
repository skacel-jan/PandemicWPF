using GalaSoft.MvvmLight;

namespace Pandemic.GameLogic
{
    public class Infection : ObservableObject
    {
        private int _position;
        private int _rate;

        public Infection()
        {
            Rate = 2;
            Position = 1;
        }

        public int Actual { get; set; }

        public int Position
        {
            get => _position;
            set => Set(ref _position, value);
        }

        public int Rate
        {
            get => _rate;
            set => Set(ref _rate, value);
        }

        public void IncreasePosition()
        {
            Position++;
            if (Position == 3 || Position == 5)
            {
                Rate++;
            }
        }

        public void Reset()
        {
            Actual = Rate;
        }
    }
}
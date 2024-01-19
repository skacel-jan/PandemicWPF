using System;

namespace Game.Pandemic.GameLogic.Actions.Effects
{
    public class OneQuietNightEffect : IEffect
    {
        private readonly Infection _infection;

        public OneQuietNightEffect(Infection infection)
        {
            _infection = infection ?? throw new ArgumentNullException(nameof(infection));
        }

        public void Execute()
        {
            _infection.Actual = 0;
        }
    }
}
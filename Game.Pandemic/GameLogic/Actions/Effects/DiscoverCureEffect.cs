namespace Game.Pandemic.GameLogic.Actions.Effects
{
    internal class DiscoverCureEffect : IEffect
    {
        private readonly Disease _disease;

        public DiscoverCureEffect(Disease disease)
        {
            _disease = disease;
        }

        public void Execute()
        {
            if (_disease.Cubes == Disease.STARTING_CUBES_COUNT)
            {
                _disease.Status = Disease.State.Eradicated;
            }
            else
            {
                _disease.Status = Disease.State.Cured;
            }
        }
    }
}
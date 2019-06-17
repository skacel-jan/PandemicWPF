namespace Pandemic.GameLogic.Actions
{
    internal class TreatDiseaseEffect : IEffect
    {
        private readonly Game _game;
        private readonly DiseaseColor _disease;
        private readonly Character _character;

        public TreatDiseaseEffect(Game game, DiseaseColor disease, Character character)
        {
            _game = game;
            _disease = disease;
            _character = character;
        }

        public void Execute()
        {
            var cubesRemoved = _character.TreatDisease(_disease);
            _game.IncreaseCubePile(_disease, cubesRemoved);
        }
    }
}
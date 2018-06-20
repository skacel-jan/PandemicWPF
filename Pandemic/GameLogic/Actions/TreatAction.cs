using System;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class TreatAction : CharacterAction
    {
        public TreatAction(Character character) : base(character)
        {
        }

        public override bool CanExecute(Game game)
        {
            return Character.CurrentMapCity.DiseasesToTreat.Count > 0;
        }

        protected override void Execute()
        {
            if (Character.CurrentMapCity.DiseasesToTreat.Count > 1)
            {
                _game.SelectDisease(Character.CurrentMapCity.DiseasesToTreat, TreatDisease, "Select disease");
            }
            else if (Character.CurrentMapCity.DiseasesToTreat.Count == 1)
            {
                TreatDisease(Character.CurrentMapCity.DiseasesToTreat.First());
            }
        }

        private void TreatDisease(DiseaseColor color)
        {
            var cubesRemoved = Character.TreatDisease(color);
            _game.IncreaseCubePile(color, cubesRemoved);

            FinishAction();
        }
    }
}
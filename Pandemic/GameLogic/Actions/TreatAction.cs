using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class TreatAction : CharacterAction
    {
        public TreatAction(Character character) : base(character)
        {
        }

        public override string Name => ActionTypes.Treat;

        public override bool CanExecute(Game game)
        {
            return Character.CurrentMapCity.DiseasesToTreat.Count > 0;
        }

        protected override void Execute()
        {
            if (Character.CurrentMapCity.DiseasesToTreat.Count > 1)
            {
                Game.SelectDisease(Character.CurrentMapCity.DiseasesToTreat, "Select disease", TreatDisease);
            }
            else if (Character.CurrentMapCity.DiseasesToTreat.Count == 1)
            {
                TreatDisease(Character.CurrentMapCity.DiseasesToTreat.First());
            }
        }

        private void TreatDisease(DiseaseColor color)
        {
            var cubesRemoved = Character.TreatDisease(color);
            Game.IncreaseCubePile(color, cubesRemoved);

            FinishAction();
        }
    }
}
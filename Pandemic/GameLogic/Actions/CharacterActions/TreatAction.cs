using System.Collections.Generic;
using System.Linq;

namespace Pandemic.GameLogic.Actions
{
    public class TreatAction : CharacterAction
    {
        private DiseaseColor _disease;

        public TreatAction(Character character, Game game) : base(character, game)
        {
        }

        public override string Name => ActionTypes.Treat;

        public override bool CanExecute()
        {
            return Character.CurrentMapCity.DiseasesToTreat.Count > 0;
        }

        protected override void AddEffects()
        {
            base.AddEffects();
            Effects.Add(new TreatDiseaseEffect(Game, _disease, Character));
        }

        protected override void Initialize()
        {
            AddSelectionState(0,
                (g) => Character.CurrentMapCity.DiseasesToTreat.Count > 1,
                new DiseaseSelection(SelectionCallback((DiseaseColor d) => _disease = d),
                                     Character.CurrentMapCity.DiseasesToTreat,
                                     "Select disease")
                );

            AddContinueState(0,
                (g) =>  Character.CurrentMapCity.DiseasesToTreat.Count == 1,
                (g) => _disease = Character.CurrentMapCity.DiseasesToTreat.First());
        }
    }
}
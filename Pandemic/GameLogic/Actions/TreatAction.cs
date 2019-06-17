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
            Effects.Add(new TreatDiseaseEffect(Game, _disease, Character));
        }

        protected override IEnumerable<Selection> PrepareSelections(Game game)
        {
            if (Character.CurrentMapCity.DiseasesToTreat.Count > 1)
            {
                yield return new DiseaseSelection(SetSelectionCallback((DiseaseColor d) => _disease = d), Character.CurrentMapCity.DiseasesToTreat, "Select disease");                
            }        
            else
            {
                _disease = Character.CurrentMapCity.DiseasesToTreat.First();
            }
        }
    }
}
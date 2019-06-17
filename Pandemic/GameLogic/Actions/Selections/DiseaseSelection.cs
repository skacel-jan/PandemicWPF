using System;
using System.Collections.Generic;

namespace Pandemic.GameLogic.Actions
{
    internal class DiseaseSelection : Selection
    {
        private readonly Action<DiseaseColor> _action;
        private readonly IList<DiseaseColor> _diseasesToTreat;
        private readonly string _infoText;

        public DiseaseSelection(Action<DiseaseColor> action, IList<DiseaseColor> diseasesToTreat, string infoText)
        {
            _action = action;
            _diseasesToTreat = diseasesToTreat;
            _infoText = infoText;
        }

        public override void Execute(SelectionService service)
        {
            service.SelectDisease(_action, _diseasesToTreat, _infoText);
        }
    }
}
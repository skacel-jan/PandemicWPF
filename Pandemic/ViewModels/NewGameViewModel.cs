using GalaSoft.MvvmLight;
using Pandemic.Characters;
using System.Collections.Generic;

namespace Pandemic.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        public NewGameViewModel()
        {
            Difficulties = new int[] { 4, 5, 6 };
            Characters = new string[] { Medic.MEDIC, OperationsExpert.OPERATIONS_EXPERT, Researcher.RESEARCHER, ContingencyPlanner.CONTINGENCY_PLANNER,
                Scientist.SCIENTIST, QuarantineSpecialist.QUARANTINE_SPECIALIST};
        }

        public IEnumerable<int> Difficulties { get; }
        public IEnumerable<string> Characters { get; }
        public int SelectedDifficulty { get; set; }
        public IEnumerable<string> SelectedCharacters { get; }
    }
}
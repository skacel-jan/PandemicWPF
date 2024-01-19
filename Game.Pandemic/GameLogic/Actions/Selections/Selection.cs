using Game.Pandemic.GameLogic.Services;

namespace Game.Pandemic.GameLogic.Actions.Selections
{
    public abstract class Selection
    {
        public string InfoText { get; set; }

        public abstract void Execute(SelectionService service);
    }
}
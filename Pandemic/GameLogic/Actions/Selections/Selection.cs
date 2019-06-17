namespace Pandemic.GameLogic.Actions
{
    public abstract class Selection
    {
        public string InfoText { get; set; }

        public abstract void Execute(SelectionService service);
    }
}
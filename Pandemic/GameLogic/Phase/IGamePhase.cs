namespace Pandemic.GameLogic
{
    public interface IGamePhase
    {
        void Start();

        void End();

        void Action(Actions.IGameAction action);

        Game Game { get; }
    }
}
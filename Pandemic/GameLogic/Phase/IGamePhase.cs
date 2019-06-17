namespace Pandemic.GameLogic
{
    public interface IGamePhase
    {
        void Start();

        void End();

        void Continue();

        Game Game { get; }
    }
}
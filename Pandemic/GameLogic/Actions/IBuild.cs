namespace Pandemic.GameLogic.Actions
{
    public interface ISelectA<T> : INext
    {
        void Set(T param);
    }

    public interface INext
    {
        Game Game { get; }

        void Next();
    }
}
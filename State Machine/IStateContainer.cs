namespace LuxStateMachine
{
    public interface IStateContainer<out TState>
    {
        TState State { get; }
    }
}
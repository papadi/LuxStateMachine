namespace LuxStateMachine
{
    public interface IStateConfigurator<in TState>
    {
        IActionConfigurator1<TState> Supports<TAction>();

        void IsTerminalState();
    }
}
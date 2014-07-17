using System.Collections.Generic;

namespace LuxStateMachine
{
    public interface IStateMachine<TState>
    {
        void Invoke<TAction>(IStateContainer<TState> stateContainer, TAction action) where TAction : class;

        bool SupportsAction<TAction>(IStateContainer<TState> stateContainer);

        IEnumerable<ActionConfiguration<TState>> GetSupportedActions(IStateContainer<TState> stateContainer);
    }
}

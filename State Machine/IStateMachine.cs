using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LuxStateMachine
{
    [ContractClass(typeof(StateMachineContracts<>))]
    public interface IStateMachine<TState>
    {
        void Invoke<TAction>(IStateContainer<TState> stateContainer, TAction action) where TAction : class;

        bool SupportsAction<TAction>(IStateContainer<TState> stateContainer);

        IEnumerable<ActionConfiguration<TState>> GetSupportedActions(IStateContainer<TState> stateContainer);
    }
}

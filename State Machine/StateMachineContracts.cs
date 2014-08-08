using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LuxStateMachine
{
    [ContractClassFor(typeof(IStateMachine<>))]
    public abstract class StateMachineContracts<TState> : IStateMachine<TState>
    {
        public void Invoke<TAction>(IStateContainer<TState> stateContainer, TAction action) where TAction : class
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);
            Contract.Requires<ArgumentNullException>(action != null);
        }

        public bool SupportsAction<TAction>(IStateContainer<TState> stateContainer)
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);
            return false;
        }

        public IEnumerable<IActionConfiguration<TState>> GetSupportedActions(IStateContainer<TState> stateContainer)
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);
            return null;
        }
    }
}
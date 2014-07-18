using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Practices.Unity;

namespace LuxStateMachine
{
    public abstract class StateMachine<TState> : IStateMachine<TState>
    {
        private readonly IDictionary<TState, IStateConfigurator<TState>> stateConfigurations = new Dictionary<TState, IStateConfigurator<TState>>();

        private readonly IUnityContainer unityContainer;

        protected StateMachine() : this(new UnityContainer())
        {
        }

        protected StateMachine(IUnityContainer unityContainer)
        {
            Contract.Requires<ArgumentNullException>(unityContainer != null);
            if (unityContainer == null) throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        protected abstract void Configure(TState state);

        protected IStateConfigurator<TState> this[TState state] { get { return this.stateConfigurations[state]; } }

        private IEnumerable<ActionConfiguration<TState>> GetActionConfigurations(Type actionType, TState state)
        {
            var stateConfiguration = this.GetStateConfiguration(state);
            return stateConfiguration.Actions.Where(p => p.ActionType == actionType && p.PreConditionsSatisfied());
        }

        public StateConfiguration<TState> GetStateConfiguration(TState state)
        {
            if (!this.stateConfigurations.ContainsKey(state))
            {
                this.stateConfigurations.Add(new KeyValuePair<TState, IStateConfigurator<TState>>(state, new StateConfiguration<TState>()));
                this.Configure(state);
            }

            if (!this.stateConfigurations.ContainsKey(state)) throw new ArgumentOutOfRangeException("state", string.Format("State '{0}' not configured", state));
            return (StateConfiguration<TState>)this.stateConfigurations[state];
        }

        #region IStateMachine Implementation

        public void Invoke<TAction>(IStateContainer<TState> stateContainer, TAction action) where TAction : class 
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);
            Contract.Requires<ArgumentNullException>(action != null);

            var stateBeforeExecution = stateContainer.State;

            var actionConfiguration = this.GetActionConfigurations(action.GetType(), stateContainer.State).FirstOrDefault();
            if (actionConfiguration == null) throw new InvalidActionException(action.GetType().Name, string.Format("Action '{0}' not allowed when state is '{1}' or conditions not met", action.GetType().Name, stateContainer.State));

            if (actionConfiguration.HandlerType == null) return;

            var resolverOverrides = new ResolverOverride[] { new ParameterOverride("item", stateContainer), new ParameterOverride("action", action), new ParameterOverride("flags", new InjectionParameter<IList>(actionConfiguration.Flags)) };

            this.unityContainer.Resolve(actionConfiguration.HandlerType, resolverOverrides);
            
            // Validate the state of the machine after the execution of the action
            var actionCanChangeState = actionConfiguration.PossibleStatesAfter != null && actionConfiguration.PossibleStatesAfter.Count > 0;
            if ((!actionCanChangeState && !stateContainer.State.Equals(stateBeforeExecution)) || (actionCanChangeState && !actionConfiguration.PossibleStatesAfter.Contains(stateContainer.State))) throw new InvalidOperationException(string.Format("Executing '{0}' on state '{1}' resulted in the invalid state '{2}'", action.GetType().Name, stateBeforeExecution, stateContainer.State));
        }

        public bool SupportsAction<TAction>(IStateContainer<TState> stateContainer)
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);

            return this.GetActionConfigurations(typeof(TAction), stateContainer.State).Any();
        }

        public IEnumerable<ActionConfiguration<TState>> GetSupportedActions(IStateContainer<TState> stateContainer)
        {
            Contract.Requires<ArgumentNullException>(stateContainer != null);

            var supportedActions = this.GetStateConfiguration(stateContainer.State).Actions.Where(p => p.PreConditionsSatisfied());

            // It is possible that an action appears twice in the supported actions (for example when it can be handled by different handlers depending on the Condition)
            // Here we keep the first one, which is the one that will be used anyway
            var comparer = new SupportedActionComparer();
            return supportedActions.Distinct(comparer);
        }

        #endregion

        private class SupportedActionComparer : IEqualityComparer<ActionConfiguration<TState>>
        {
            public bool Equals(ActionConfiguration<TState> x, ActionConfiguration<TState> y)
            {
                if (x == null || y == null) return false;
                return x.ActionType == y.ActionType;
            }

            public int GetHashCode(ActionConfiguration<TState> obj)
            {
                return obj == null ? 0 : obj.ActionType.GetHashCode();
            }
        }
    }
}
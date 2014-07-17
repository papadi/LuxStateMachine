using System;
using System.Collections.Generic;

namespace LuxStateMachine
{
    public class StateConfiguration<TState> : IStateConfigurator<TState>
    {
        public StateConfiguration()
        {
            this.Actions = new List<ActionConfiguration<TState>>();
        }

        public IList<ActionConfiguration<TState>> Actions { get; private set; }

        public IActionConfigurator1<TState> Supports<TAction>()
        {
            var actionConfiguration = new ActionConfiguration<TState>(typeof(TAction));
            this.Actions.Add(actionConfiguration);
            return actionConfiguration;
        }

        public void IsTerminalState()
        {
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LuxStateMachine
{
    public class ActionConfiguration<TState> : IActionConfigurator1<TState>, IActionConfigurator2<TState>, IActionConfiguration<TState>
    {
        public ActionConfiguration(Type actionType)
        {
            this.ActionType = actionType;
            this.PossibleStatesAfter = new List<TState>();
            this.Flags = new ArrayList();
        }

        public Type ActionType { get; private set; }

        public Type HandlerType { get; private set; }

        public List<TState> PossibleStatesAfter { get; private set; }

        public ArrayList Flags { get; private set; }

        public Func<bool> PreCondition { get; private set; }

        public IList<string> UserRoles { get; private set; }

        public IActionConfigurator1<TState> If(Func<bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            this.PreCondition = predicate;
            return this;
        }

        public IActionConfigurator1<TState> IfUserInRole(params string[] roles)
        {
            this.UserRoles = roles;
            return this;
        }

        public IActionConfigurator2<TState> ResultsInStates(params TState[] states)
        {
            this.PossibleStatesAfter.AddRange(states);
            return this;
        }

        public IActionConfigurator2<TState> HandledBy<TActionHandler>()
        {
            this.HandlerType = typeof(TActionHandler);
            return this;
        }

        public IActionConfigurator2<TState> DoesNotChangeState()
        {
            this.PossibleStatesAfter.Clear();
            return this;
        }

        public IActionConfigurator2<TState> WithFlags(params object[] flags)
        {
            this.Flags.AddRange(flags);
            return this;
        }

        internal bool PreConditionsSatisfied()
        {
            var rolesResult = true; // this.UserRoles == null || this.UserRoles.Any(userRole => Thread.CurrentPrincipal.IsInRole(userRole));
            var preConditionResult = this.PreCondition == null || this.PreCondition();

            return rolesResult && preConditionResult;
        }
    }
}
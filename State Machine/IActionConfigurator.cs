using System;

namespace LuxStateMachine
{
    public interface IActionConfigurator1<in TState>
    {
        IActionConfigurator2<TState> HandledBy<TActionHandler>();

        IActionConfigurator1<TState> If(Func<bool> predicate);

        IActionConfigurator1<TState> IfUserInRole(params string[] roles);
    }

    public interface IActionConfigurator2<in TState>
    {
        IActionConfigurator2<TState> ResultsInStates(params TState[] states);

        IActionConfigurator2<TState> DoesNotChangeState();

        IActionConfigurator2<TState> WithFlags(params object[] flags);
    }
}

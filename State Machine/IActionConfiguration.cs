using System;
using System.Collections;
using System.Collections.Generic;

namespace LuxStateMachine
{
    public interface IActionConfiguration<TState>
    {
        Type ActionType { get; }

        Type HandlerType { get; }

        List<TState> PossibleStatesAfter { get; }

        ArrayList Flags { get; }

        Func<bool> PreCondition { get; }

        IList<string> UserRoles { get; }
    }
}
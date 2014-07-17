using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuxStateMachine;
using MSTestExtensions;

namespace OrderSample.UnitTests
{
    [TestClass]
    public class StateMachineSpecialTest
    {
        [TestMethod]
        public void UnconfiguredState()
        {
            var testworkflow = new TestWorkflow();
            var testEntity = new StateContainer { State = State.Value3 };

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => testworkflow.Invoke(testEntity, new { }));
        }

        [TestMethod]
        public void NullAction()
        {
            var testworkflow = new TestWorkflow();
            var testEntity = new StateContainer { State = State.Value1 };
            object testAction = null;

            ExceptionAssert.Throws<ArgumentNullException>(() => testworkflow.Invoke(testEntity, testAction));
        }
    }

    public enum State
    {
        Value1,
        Value2,
        Value3
    }

    public class StateContainer : IStateContainer<State>
    {
        public State State { get; set; }
    }

    public class TestWorkflow : StateMachine<State>
    {
        protected override void Configure(State state)
        {
            switch (state)
            {
                case State.Value1:
                    break;
                case State.Value2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}

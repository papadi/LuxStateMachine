using System;
using OrderSample.StateMachine.Actions;
using OrderSample.DomainObjects;
using OrderSample.StateMachine.Handlers;
using LuxStateMachine;

namespace OrderSample.StateMachine
{
    public class OrderStateMachine : StateMachine<OrderState>
    {
        const string EmployeeGroupName = "EMPLOYEE";

        protected override void Configure(OrderState state)
        {
            switch (state)
            {
                case OrderState.New:
                    this[state].Supports<CreateNew>().HandledBy<CreateNewHandler>().ResultsInStates(OrderState.Submitted);
                    break;
                case OrderState.Submitted:
                    this[state].Supports<Cancel>().HandledBy<CancelHandler>().ResultsInStates(OrderState.Canceled);
                    this[state].Supports<Update>().HandledBy<UpdateHandler>().DoesNotChangeState();
                    this[state].Supports<MarkAsPaidWithCash>().IfUserInRole(EmployeeGroupName).HandledBy<MarkAsPaidWithCashHandler>().ResultsInStates(OrderState.Paid);
                    this[state].Supports<PayWithCreditCard>().HandledBy<PayWithCreditCardHandler>().ResultsInStates(OrderState.Paid);
                    break;
                case OrderState.Paid:
                    this[state].Supports<MarkAsShipped>().IfUserInRole(EmployeeGroupName).HandledBy<MarkAsShippedHandler>().ResultsInStates(OrderState.Shipped);
                    break;
                case OrderState.Shipped:
                    this[state].IsTerminalState();
                    break;
                case OrderState.Canceled:
                    this[state].IsTerminalState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}

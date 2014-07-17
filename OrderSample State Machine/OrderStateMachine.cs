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
                    this[state].Supports<Cancel>().IfUserInRole(EmployeeGroupName).HandledBy<CancelHandler>().ResultsInStates(OrderState.Canceled);
                    this[state].Supports<Cancel>().HandledBy<CancelRequestHandler>().ResultsInStates(OrderState.CancelationRequested);
                    this[state].Supports<Update>().HandledBy<UpdateHandler>().DoesNotChangeState();
                    this[state].Supports<MarkAsPaidWithCash>().HandledBy<MarkAsPaidWithCashHandler>().ResultsInStates(OrderState.Paid);
                    this[state].Supports<Pay>().HandledBy<PayHandler>().ResultsInStates(OrderState.Paid);
                    break;
                case OrderState.CancelationRequested:
                    this[state].Supports<Cancel>().IfUserInRole(EmployeeGroupName).HandledBy<CancelHandler>().ResultsInStates(OrderState.Canceled);
                    break;
                case OrderState.Paid:
                    this[state].Supports<Cancel>().IfUserInRole(EmployeeGroupName).HandledBy<CancelHandler>().ResultsInStates(OrderState.Canceled);
                    this[state].Supports<MarkAsShipped>().HandledBy<MarkAsShippedHandler>().ResultsInStates(OrderState.Shipped);
                    break;
                case OrderState.Shipped:
                    this[state].Supports<Cancel>().IfUserInRole(EmployeeGroupName).HandledBy<CancelHandler>().ResultsInStates(OrderState.Canceled);
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

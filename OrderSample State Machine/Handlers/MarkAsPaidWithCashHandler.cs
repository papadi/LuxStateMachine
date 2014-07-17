using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class MarkAsPaidWithCashHandler
    {
        public MarkAsPaidWithCashHandler(Order item)
        {
            item.State = OrderState.Paid;
        }
    }
}

using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class MarkAsShippedHandler
    {
        public MarkAsShippedHandler(Order item)
        {
            item.State = OrderState.Shipped;
        }
    }
}

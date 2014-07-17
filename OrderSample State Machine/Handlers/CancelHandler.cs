using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class CancelHandler
    {
        public CancelHandler(Order item)
        {
            item.State = OrderState.Canceled;
        }
    }
}

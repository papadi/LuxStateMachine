using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class CancelRequestHandler
    {
        public CancelRequestHandler(Order item)
        {
            item.State = OrderState.CancelationRequested;
        }
    }
}

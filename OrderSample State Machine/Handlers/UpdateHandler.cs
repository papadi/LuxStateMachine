using OrderSample.StateMachine.Actions;
using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class UpdateHandler
    {
        public UpdateHandler(Update action, Order item)
        {
            // Update CustomerAddress if specified
            if (!string.IsNullOrEmpty(action.CustomerAddress))
            {
                item.CustomerAddress = action.CustomerAddress;
            }

            // Update NumberOfItems if specified
            if (action.NumberOfItems.HasValue)
            {
                item.NumberOfItems = action.NumberOfItems.Value;

                // Calculate the new total price
                item.TotalPrice = action.NumberOfItems.Value * CreateNewHandler.ItemPrice;
            }
        }
    }
}

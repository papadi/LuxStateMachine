using System;
using OrderSample.StateMachine.Actions;
using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class CreateNewHandler
    {
        public const double ItemPrice = 10;

        public CreateNewHandler(CreateNew action, Order item)
        {
            // Validate the request
            // TODO : Write validation code here

            // Set some initial data
            item.Id = Guid.NewGuid();
            item.DateCreated = DateTime.Now;
            item.State = OrderState.Submitted;

            item.CustomerName = action.CustomerName;
            item.CustomerAddress = action.CustomerAddress;
            item.NumberOfItems = action.NumberOfItems;

            item.TotalPrice = action.NumberOfItems*ItemPrice;
        }
    }
}

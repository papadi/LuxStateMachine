using System;
using OrderSample.StateMachine.Actions;
using OrderSample.DomainObjects;

namespace OrderSample.StateMachine.Handlers
{
    public class PayWithCreditCardHandler
    {
        public PayWithCreditCardHandler(PayWithCreditCard action, Order item)
        {
            // Validate the card
            if (action.CreditCardExpirationDate < DateTime.Now) throw new InvalidOperationException("Credit card has expired");

            // Process the payment
            // TODO : Make the credit card payment here

            // Mark order as paid
            item.State = OrderState.Paid;
        }
    }
}

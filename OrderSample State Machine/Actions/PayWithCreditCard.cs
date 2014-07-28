using System;

namespace OrderSample.StateMachine.Actions
{
    public class PayWithCreditCard
    {
        public string CreditCardNumber { get; set; }

        public string CreditCardOwner { get; set; }

        public string CreditCardSecurityCode { get; set; }

        public DateTime CreditCardExpirationDate { get; set; }
    }
}

using System;
using System.Linq;
using LuxStateMachine;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestExtensions;
using OrderSample.DomainObjects;
using OrderSample.StateMachine;
using OrderSample.StateMachine.Actions;

namespace OrderSample.UnitTests
{
    [TestClass]
    public class OrderStateMachineUnitTest
    {
        [TestMethod]
        public void TestNewOrder()
        {
            var order = new Order();
            IStateMachine<OrderState> stateMachine = new OrderStateMachine();

            // Case 1 : Cannot apply pay action on new order
            Assert.AreEqual(false, stateMachine.SupportsAction<MarkAsPaidWithCash>(order));
            ExceptionAssert.Throws<InvalidActionException>(() =>
            {
                var prematurePaymentAction = new MarkAsPaidWithCash();
                stateMachine.Invoke(order, prematurePaymentAction);
            });

            // Case 2 : Initialise/create the new order
            var createNewAction = new CreateNew { CustomerName = "Black Jack", CustomerAddress = "8 Route de Luxembourg, L-1111 Luxembourg", NumberOfItems = 3 };
            stateMachine.Invoke(order, createNewAction);

            Assert.AreEqual("Black Jack", order.CustomerName);
            Assert.AreEqual(3, order.NumberOfItems);
            Assert.AreEqual(30, order.TotalPrice);
            Assert.AreEqual(OrderState.Submitted, order.State);

            // Case 3 : Get Possible actions after Submitted state
            var possibleActions = stateMachine.GetSupportedActions(order).ToArray();

            Assert.AreEqual(4, possibleActions.Length);
            Assert.IsTrue(possibleActions.Any(p => p.ActionType == typeof(Cancel)));
            Assert.IsTrue(possibleActions.Any(p => p.ActionType == typeof(MarkAsPaidWithCash)));
            Assert.IsTrue(possibleActions.Any(p => p.ActionType == typeof(Update)));
            Assert.IsTrue(possibleActions.Any(p => p.ActionType == typeof(PayWithCreditCard)));

            // Case 4 : Update
            var updateAction = new Update { NumberOfItems = 4 };
            stateMachine.Invoke(order, updateAction);

            // Assert the new state
            Assert.AreEqual(4, order.NumberOfItems);
            Assert.AreEqual(40, order.TotalPrice);

            // Case 5 : Perform payment with invalid card
            ExceptionAssert.Throws<ResolutionFailedException>(() =>
            {
                var invalidPayAction = new PayWithCreditCard { CreditCardNumber = "11112222223333", CreditCardExpirationDate = DateTime.Today.AddYears(-1), CreditCardSecurityCode = "111", CreditCardOwner = "Black Jack" };
                stateMachine.Invoke(order, invalidPayAction);
            });

            // State shouldn't have changed
            Assert.AreEqual(OrderState.Submitted, order.State);

            // Case 6 : Perform payment with valid card
            var payAction = new PayWithCreditCard { CreditCardNumber = "11112222223333", CreditCardExpirationDate = DateTime.Today.AddYears(1), CreditCardSecurityCode = "111", CreditCardOwner = "Black Jack" };
            stateMachine.Invoke(order, payAction);

            // Assert the new state
            Assert.AreEqual(OrderState.Paid, order.State);

            // Case 7 : Get Possible actions after payment
            possibleActions = stateMachine.GetSupportedActions(order).ToArray();
            Assert.AreEqual(1, possibleActions.Length);
            Assert.IsTrue(possibleActions.Any(p => p.ActionType == typeof(MarkAsShipped)));
        }
    }
}

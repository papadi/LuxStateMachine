using System;
using LuxStateMachine;

namespace OrderSample.DomainObjects
{
    public class Order : IStateContainer<OrderState>
    {
        public Order()
        {
            this.Id = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
            this.State = OrderState.New;
        }

        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateShipped { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public int NumberOfItems { get; set; }

        public double TotalPrice { get; set; }

        public OrderState State { get; set; }
    }
}

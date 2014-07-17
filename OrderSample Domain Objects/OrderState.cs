namespace OrderSample.DomainObjects
{
    public enum OrderState
    {
        New,
        Submitted,
        Paid,
        Shipped,
        CancelationRequested,
        Canceled
    }
}

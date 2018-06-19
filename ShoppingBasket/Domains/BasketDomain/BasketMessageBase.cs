namespace ShoppingBasket.Domains.BasketDomain
{
    public abstract class BasketMessageBase : MessageBase
    {
        public int CustomerId { get; }

        protected BasketMessageBase(ulong correlationId, int customerId) : base(correlationId)
        {
            CustomerId = customerId;
        }
    }
}
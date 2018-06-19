namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class NotEnoughItemsInStock : BasketMessageBase
    {
        public uint AmountAvailable { get; }

        public NotEnoughItemsInStock(ulong correlationId, int customerId, uint amountAvailable) : base(correlationId, customerId)
        {
            AmountAvailable = amountAvailable;
        }
    }
}
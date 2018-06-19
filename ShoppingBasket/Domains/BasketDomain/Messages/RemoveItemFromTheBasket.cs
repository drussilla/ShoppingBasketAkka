namespace ShoppingBasket.Domains.BasketDomain.Messages
{
    public class RemoveItemFromTheBasket : BasketMessageBase
    {
        public int ProductId { get; }

        public RemoveItemFromTheBasket(ulong correlationId, int customerId, int productId)
            : base(correlationId, customerId)
        {
            ProductId = productId;
        }
    }
}
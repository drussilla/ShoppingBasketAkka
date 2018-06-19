namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class NotEnoughItemsInStock : MessageBase
    {
        public uint ItemsAvailableInStock { get; }

        public NotEnoughItemsInStock(ulong correlationId, uint itemsAvailableInStock) : base(correlationId)
        {
            ItemsAvailableInStock = itemsAvailableInStock;
        }
    }
}
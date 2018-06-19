namespace ShoppingBasket.Domains.ProductDomain.Messages
{
    public class PutProductInStock : MessageBase
    {
        public int ProductId { get; }
        public uint Amount { get; }

        public PutProductInStock(ulong correlationId, int productId, uint amount) : base(correlationId)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
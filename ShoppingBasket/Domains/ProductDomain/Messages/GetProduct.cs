namespace ShoppingBasket.Domains.ProductDomain.Messages
{
    public class GetProduct: MessageBase
    {
        public int ProductId { get; }

        public GetProduct(ulong correlationId, int productId) : base(correlationId)
        {
            ProductId = productId;
        }
    }
}
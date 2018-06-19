namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class ProductNotFound : MessageBase
    {
        public ProductNotFound(ulong correlationId) : base(correlationId)
        {
        }
    }
}
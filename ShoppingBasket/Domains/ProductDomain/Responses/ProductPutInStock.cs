namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class ProductPutInStock : MessageBase
    {
        public ProductPutInStock(ulong correlationId) : base(correlationId)
        {
        }
    }
}
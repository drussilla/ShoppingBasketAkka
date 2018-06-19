namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class ProductFound : MessageBase
    {
        public Product Product { get; }

        public ProductFound(ulong correlationId, Product product) : base(correlationId)
        {
            Product = product;
        }
    }
}
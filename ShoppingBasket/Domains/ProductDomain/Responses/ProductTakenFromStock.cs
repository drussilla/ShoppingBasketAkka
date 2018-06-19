namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class ProductTakenFromStock : MessageBase
    {
        public ProductTakenFromStock(ulong correlationId) : base(correlationId)
        {
        }
    }
}
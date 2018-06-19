namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class ProductNotFound : BasketMessageBase
    {
        public ProductNotFound(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
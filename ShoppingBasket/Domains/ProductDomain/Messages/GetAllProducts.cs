namespace ShoppingBasket.Domains.ProductDomain.Messages
{
    public class GetAllProducts : MessageBase
    {
        public GetAllProducts(ulong correlationId) : base(correlationId)
        {
        }
    }
}
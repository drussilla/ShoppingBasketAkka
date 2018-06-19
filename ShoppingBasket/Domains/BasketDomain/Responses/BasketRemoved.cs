namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class BasketRemoved : BasketMessageBase
    {
        public BasketRemoved(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
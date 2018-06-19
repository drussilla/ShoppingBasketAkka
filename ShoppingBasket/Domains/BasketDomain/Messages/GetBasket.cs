namespace ShoppingBasket.Domains.BasketDomain.Messages
{
    public class GetBasket : BasketMessageBase
    {
        public GetBasket(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
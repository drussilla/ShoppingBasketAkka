namespace ShoppingBasket.Domains.BasketDomain.Messages
{
    public class RemoveBasket : BasketMessageBase 
    {
        public RemoveBasket(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
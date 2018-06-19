namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class ItemRemovedFromTheBasket : BasketMessageBase
    {
        public ItemRemovedFromTheBasket(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
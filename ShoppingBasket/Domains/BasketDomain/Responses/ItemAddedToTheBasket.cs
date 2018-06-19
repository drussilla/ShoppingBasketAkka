namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class ItemAddedToTheBasket : BasketMessageBase
    {
        public ItemAddedToTheBasket(ulong correlationId, int customerId) : base(correlationId, customerId)
        {
        }
    }
}
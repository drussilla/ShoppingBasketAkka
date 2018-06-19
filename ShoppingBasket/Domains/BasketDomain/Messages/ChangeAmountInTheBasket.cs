namespace ShoppingBasket.Domains.BasketDomain.Messages
{
    public class ChangeAmountInTheBasket: BasketMessageBase
    {
        public ChangeAmountInTheBasket(ulong correlationId, int customerId) 
            : base(correlationId, customerId)
        {
        }
    }
}
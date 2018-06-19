namespace ShoppingBasket.Domains.BasketDomain.Messages
{
    public class AddItemToTheBasket : BasketMessageBase
    {
        public int ProductId { get; }
        public uint Amount { get; }

        public AddItemToTheBasket(ulong correlationId, int customerId, uint amount, int productId) 
            : base(correlationId, customerId)
        {
            Amount = amount;
            ProductId = productId;
        }
    }
}
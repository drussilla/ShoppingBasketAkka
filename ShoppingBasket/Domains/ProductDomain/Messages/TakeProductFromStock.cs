namespace ShoppingBasket.Domains.ProductDomain.Messages
{
    public class TakeProductFromStock: MessageBase
    {
        public int ProductId { get; }
        public uint AmountToTakeFromStock { get; }

        public TakeProductFromStock(ulong correlationId, int productId, uint amountToTakeFromStock) 
            : base(correlationId)
        {
            ProductId = productId;
            AmountToTakeFromStock = amountToTakeFromStock;
        }
    }
}
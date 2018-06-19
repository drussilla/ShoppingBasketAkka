namespace ShoppingBasket.Domains.ProductDomain
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double ItemPrice { get; set; }

        public uint AmountInStock { get; set; }
    }
}
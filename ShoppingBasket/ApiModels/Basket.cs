using System.Collections.Generic;

namespace ShoppingBasket.ApiModels
{
    public class Basket
    {
        public int CustomerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
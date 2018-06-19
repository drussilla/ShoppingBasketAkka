using System.Collections.Generic;

namespace ShoppingBasket.Domains.ProductDomain
{
    public class ProductsDataSource: IProductsDataSource
    {
        private readonly IList<Product> _all = new List<Product>
        {
            new Product
            {
                Id = 1,
                Title = "iPhone 6",
                AmountInStock = 10,
                ItemPrice = 500.95f
            },
            new Product
            {
                Id = 2,
                Title = "iPhone X",
                AmountInStock = 0,
                ItemPrice = 999f
            },
            new Product
            {
                Id = 3,
                Title = "Samsung 9s+",
                AmountInStock = 1,
                ItemPrice = 845.95f
            },
            new Product
            {
                Id = 4,
                Title = "Samsung 9s",
                AmountInStock = 3,
                ItemPrice = 750.50f
            },
            new Product
            {
                Id = 5,
                Title = "One+ 6",
                AmountInStock = 999,
                ItemPrice = 750.50f
            },
            new Product
            {
                Id = 6,
                Title = "Nokia 3310",
                AmountInStock = 1,
                ItemPrice = 9999f
            }
        };

        public IList<Product> GetAll()
        {
            return _all;
        }
    }
}
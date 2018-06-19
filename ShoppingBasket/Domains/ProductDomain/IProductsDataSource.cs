using System.Collections.Generic;

namespace ShoppingBasket.Domains.ProductDomain
{
    public interface IProductsDataSource
    {
        IList<Product> GetAll();
    }
}
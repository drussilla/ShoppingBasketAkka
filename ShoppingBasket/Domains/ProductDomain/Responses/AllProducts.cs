using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShoppingBasket.Domains.ProductDomain.Responses
{
    public class AllProducts : MessageBase
    {
        public IReadOnlyCollection<Product> Products { get; }

        public AllProducts(ulong correlationId, ICollection<Product> products) : base(correlationId)
        {
            Products = new ReadOnlyCollection<Product>(products.ToList().AsReadOnly());
        }
    }
}
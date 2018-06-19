using System.Collections.Generic;

namespace ShoppingBasket.Domains.BasketDomain.Responses
{
    public class BasketFound : BasketMessageBase
    {
        public IDictionary<int, uint> Items { get; }

        public BasketFound(ulong correlationId, int customerId, IDictionary<int, uint> items) : base(correlationId, customerId)
        {
            Items = items;
        }
    }
}
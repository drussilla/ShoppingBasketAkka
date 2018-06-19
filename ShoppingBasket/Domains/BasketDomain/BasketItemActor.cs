using Akka.Actor;
using System;

namespace ShoppingBasket.Domains.BasketDomain
{
    public class BasketItemActor : ReceiveActor
    {
        public Guid Id { get; set; }

        public int ProductId { get; set; }

        public uint Amount { get; set; }
    }
}
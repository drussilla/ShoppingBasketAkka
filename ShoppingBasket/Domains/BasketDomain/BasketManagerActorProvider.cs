using Akka.Actor;
using ShoppingBasket.Domains.ProductDomain;

namespace ShoppingBasket.Domains.BasketDomain
{
    public class BasketManagerActorProvider
    {
        private readonly IActorRef _basketManagerRef;

        public BasketManagerActorProvider(IActorRefFactory system, ProductActorProvider productActorProvider)
        {
            _basketManagerRef = system.ActorOf(BasketManagerActor.Props(productActorProvider.Get()), "basket-manager");
        }

        public IActorRef Get() => _basketManagerRef;
    }
}
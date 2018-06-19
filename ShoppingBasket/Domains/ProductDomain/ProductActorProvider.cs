using Akka.Actor;

namespace ShoppingBasket.Domains.ProductDomain
{
    public class ProductActorProvider
    {
        private readonly IActorRef _productActorRef;

        public ProductActorProvider(IActorRefFactory system, IProductsDataSource productsDataSource)
        {
            _productActorRef = system.ActorOf(ProductActor.Props(productsDataSource), "products");
        }

        public IActorRef Get() => _productActorRef;
    }
}
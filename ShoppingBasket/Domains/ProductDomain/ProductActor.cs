using Akka.Actor;
using System.Collections.Generic;
using System.Linq;
using ShoppingBasket.Domains.ProductDomain.Messages;
using ShoppingBasket.Domains.ProductDomain.Responses;

namespace ShoppingBasket.Domains.ProductDomain
{
    public class ProductActor : ReceiveActor
    {
        private readonly IDictionary<int, Product> _products;

        public static Props Props(IProductsDataSource productsDataSource) =>
            Akka.Actor.Props.Create(() => new ProductActor(productsDataSource));

        public ProductActor(IProductsDataSource productsDataSource)
        {
            _products = productsDataSource.GetAll().ToDictionary(key => key.Id, value => value);

            Receive<GetProduct>(message => Sender.Tell(GetProduct(message)));
            Receive<GetAllProducts>(message => Sender.Tell(new AllProducts(message.CorrelationId, _products.Values)));
            Receive<TakeProductFromStock>(message => Sender.Tell(TakeProductFromStock(message)));
            Receive<PutProductInStock>(message => Sender.Tell(PutProductInStock(message)));
        }

        private MessageBase GetProduct(GetProduct message)
        {
            if (!_products.TryGetValue(message.ProductId, out var product))
            {
                return new ProductNotFound(message.CorrelationId);
            }

            return new ProductFound(message.CorrelationId, product);
        }

        private MessageBase TakeProductFromStock(TakeProductFromStock message)
        {
            if (!_products.TryGetValue(message.ProductId, out var product))
            {
                return new ProductNotFound(message.CorrelationId);
            }

            // not enough items in stock to make a change
            if ((int)(product.AmountInStock - message.AmountToTakeFromStock) < 0)
            {
                return new NotEnoughItemsInStock(message.CorrelationId, product.AmountInStock);
            }

            product.AmountInStock -= message.AmountToTakeFromStock;
            return new ProductTakenFromStock(message.CorrelationId);
        }

        private MessageBase PutProductInStock(PutProductInStock message)
        {
            if (!_products.TryGetValue(message.ProductId, out var product))
            {
                return new ProductNotFound(message.CorrelationId);
            }

            product.AmountInStock += message.Amount;
            return new ProductPutInStock(message.CorrelationId);
        }
    }
}
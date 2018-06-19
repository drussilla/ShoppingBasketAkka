using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using ShoppingBasket.Domains.BasketDomain.Messages;
using ShoppingBasket.Domains.BasketDomain.Responses;
using ShoppingBasket.Domains.ProductDomain.Messages;
using ShoppingBasket.Domains.ProductDomain.Responses;

namespace ShoppingBasket.Domains.BasketDomain
{
    public class BasketActor : ReceiveActor
    {
        private readonly IActorRef _productActor;

        // TODO: I decided to store simple ProductId -> Amount mapping instead of separate 
        // collection of BasketItems because we do not know if there is business domain requirement
        // to store state of the product when you put it in the basket or it should 
        // be only fixed at the moment you pay for products in the basket
        // if needed this approach can be extended with separate BasketItemActor that will 
        // store "snapshot" of the product
        private readonly IDictionary<int, uint> _productIdToAmountMap = new Dictionary<int, uint>();

        public static Props Props(int customerId, IActorRef productActor) =>
            Akka.Actor.Props.Create(() => new BasketActor(customerId, productActor));

        public BasketActor(int customerId, IActorRef productActor)
        {
            _productActor = productActor;

            ReceiveAsync<AddItemToTheBasket>(m => AddItemToTheBasket(m).PipeTo(Sender),
                m => m.CustomerId == customerId);

            Receive<GetBasket>(m => Sender.Tell(new BasketFound(m.CorrelationId, m.CustomerId, _productIdToAmountMap)),
                m => m.CustomerId == customerId);

            ReceiveAsync<RemoveItemFromTheBasket>(m => RemoveItemFromTheBasket(m).PipeTo(Sender),
                m => m.CustomerId == customerId);

            Receive<RemoveBasket>(m =>
            {
                Self.GracefulStop(TimeSpan.FromSeconds(1));
                Sender.Tell(new BasketRemoved(m.CorrelationId, m.CustomerId));
            });
        }

        private async Task<MessageBase> AddItemToTheBasket(AddItemToTheBasket message)
        {
            var response =
                await _productActor.Ask<MessageBase>(new TakeProductFromStock(message.CorrelationId, message.ProductId,
                    message.Amount));

            switch (response)
            {
                case ProductTakenFromStock _:
                    UpdateBasket(message.ProductId, message.Amount);
                    return new ItemAddedToTheBasket(message.CorrelationId, message.CustomerId);
                case ProductDomain.Responses.ProductNotFound _:
                    return new Responses.ProductNotFound(message.CorrelationId, message.CustomerId);
                case ProductDomain.Responses.NotEnoughItemsInStock notEnough:
                    return new Responses.NotEnoughItemsInStock(message.CorrelationId, message.CustomerId, notEnough.ItemsAvailableInStock);
                default:
                    throw new NotImplementedException($"Unsupported response: {response.GetType()}");
            }
        }

        private async Task<MessageBase> RemoveItemFromTheBasket(RemoveItemFromTheBasket message)
        {
            if (!_productIdToAmountMap.TryGetValue(message.ProductId, out var amount))
            {
                return new Responses.ProductNotFound(message.CorrelationId, message.CustomerId);
            }

            var response =
                await _productActor.Ask<MessageBase>(
                    new PutProductInStock(
                        message.CorrelationId, 
                        message.ProductId,
                        amount));

            switch (response)
            {
                case ProductDomain.Responses.ProductNotFound _:
                    return new Responses.ProductNotFound(message.CorrelationId, message.CustomerId);
                case ProductPutInStock _:
                    _productIdToAmountMap.Remove(message.ProductId);
                    return new ItemRemovedFromTheBasket(message.CorrelationId, message.CustomerId);
                default:
                    throw new NotImplementedException($"Unsupported response: {response.GetType()}");
            }
        }

        private void UpdateBasket(int productId, uint amount)
        {
            if (!_productIdToAmountMap.ContainsKey(productId))
            {
                _productIdToAmountMap[productId] = amount;
            }
            else
            {
                _productIdToAmountMap[productId] += amount;
            }
        }
    }
}
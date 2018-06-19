using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;

namespace ShoppingBasket.Domains.BasketDomain
{
    public class BasketManagerActor : ReceiveActor
    {
        private readonly IActorRef _productActor;
        private readonly Dictionary<int, IActorRef> _customerIdToBasketMap = new Dictionary<int, IActorRef>();
        private readonly Dictionary<IActorRef, int> _basketToCustomerIdMap = new Dictionary<IActorRef, int>();

        protected ILoggingAdapter Log { get; } = Context.GetLogger();

        public static Props Props(IActorRef productActor) =>
            Akka.Actor.Props.Create(() => new BasketManagerActor(productActor));

        public BasketManagerActor(IActorRef productActor)
        {
            _productActor = productActor;
            Receive<BasketMessageBase>(m => ForwardToBasket(m));
            Receive<Terminated>(m => RemoveBasket(m));
        }

        private void RemoveBasket(Terminated terminated)
        {
            if (_basketToCustomerIdMap.TryGetValue(terminated.ActorRef, out var customerId))
            {
                _basketToCustomerIdMap.Remove(terminated.ActorRef);
                _customerIdToBasketMap.Remove(customerId);

                Log.Info($"Basket for customer {customerId} has been removed");
            }
        }

        private void ForwardToBasket(BasketMessageBase message)
        {
            if (!_customerIdToBasketMap.TryGetValue(message.CustomerId, out var basketActor))
            {
                // if basket with unknown customer id is requested we will create basket for that customer id.
                // logic might be a bit different in the real application
                basketActor = Context.ActorOf(BasketActor.Props(message.CustomerId, _productActor), $"basket-{message.CustomerId}");
                _customerIdToBasketMap[message.CustomerId] = basketActor;
                _basketToCustomerIdMap[basketActor] = message.CustomerId;

                Context.Watch(basketActor);

                Log.Info($"Basket for customer {message.CustomerId} has been created");
            }

            basketActor.Forward(message);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingBasket.ApiModels;
using ShoppingBasket.Domains.BasketDomain;
using ShoppingBasket.Domains.BasketDomain.Messages;
using ShoppingBasket.Domains.BasketDomain.Responses;

namespace ShoppingBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _log;
        private readonly IActorRef _basketManagerActor;

        public BasketController(ILogger<BasketController> log, BasketManagerActorProvider actorProvider)
        {
            _log = log;
            _basketManagerActor = actorProvider.Get();
        }

        // GET api/basket/1
        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<Basket>>> Get(int customerId)
        {
            _log.LogInformation($"Get basket for customer {customerId}");
            var response = await _basketManagerActor.Ask<BasketFound>(new GetBasket(1, customerId));

            // we can use auto mapper to map domain object to API layer objects
            return Ok(new Basket
            {
                CustomerId = customerId,
                Items = response.Items.Select(x => 
                    new BasketItem
                    {
                        ProductId = x.Key,
                        Amount = x.Value
                    }).ToList()
            });
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> Delete(int customerId)
        {
            _log.LogInformation($"Delete basket for customer with id {customerId}");
            await _basketManagerActor.Ask<BasketRemoved>(new RemoveBasket(4, customerId));

            return Ok();
        }

        // PUT api/basket/{customerId}/item
        [HttpPut("{customerId}/item")]
        public async Task<ActionResult<IEnumerable<Basket>>> PutItem(int customerId, [FromBody] BasketItem item)
        {
            _log.LogInformation($"Add product {item.ProductId}");
            var response = await _basketManagerActor.Ask(new AddItemToTheBasket(1, customerId, item.Amount, item.ProductId));

            switch (response)
            {
                case ItemAddedToTheBasket _:
                    return Ok();
                case ProductNotFound _:
                    return BadRequest($"Product with id {item.ProductId} not found");
                case NotEnoughItemsInStock notEnough:
                    return BadRequest($"Not enough items in stock. Only {notEnough.AmountAvailable} available");
            }

            return BadRequest();
        }

        [HttpDelete("{customerId}/item/{productId}")]
        public async Task<ActionResult> DeleteItem(int customerId, int productId)
        {
            _log.LogInformation($"Delete product {productId} from basket {customerId}");

            var response = await _basketManagerActor.Ask(new RemoveItemFromTheBasket(2, customerId, productId));

            switch (response)
            {
                case ItemRemovedFromTheBasket _:
                    return Ok();
                case ProductNotFound _:
                    return BadRequest($"Product with id {productId} not found in basket {customerId}");
            }

            return BadRequest();
        }
    }
}

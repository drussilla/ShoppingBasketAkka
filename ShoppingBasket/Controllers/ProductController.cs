using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Domains.ProductDomain;
using ShoppingBasket.Domains.ProductDomain.Messages;
using ShoppingBasket.Domains.ProductDomain.Responses;

namespace ShoppingBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IActorRef _productActor;

        public ProductController(ILogger<ProductController> log, ProductActorProvider actorProvider)
        {
            _productActor = actorProvider.Get();
        }

        // GET api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var response = await _productActor.Ask<AllProducts>(new GetAllProducts(1));
            return Ok(response.Products);
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var response = await _productActor.Ask(new GetProduct(2, id));
            switch (response)
            {
                case ProductFound found:
                    return Ok(found.Product);
                case ProductNotFound _:
                    return NotFound();
            }

            return BadRequest();
        }
    }
}

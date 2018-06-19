using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit;
using FluentAssertions;
using Moq;
using ShoppingBasket.Domains.BasketDomain;
using ShoppingBasket.Domains.BasketDomain.Messages;
using ShoppingBasket.Domains.BasketDomain.Responses;
using ShoppingBasket.Domains.ProductDomain;
using Xunit;

namespace Domains.Tests
{
    public class BasketActorShould : TestKit
    {
        private Product GetProduct() => new Product
        {
            Id = 1,
            Title = "test",
            AmountInStock = 1,
            ItemPrice = 999
        };

        [Fact]
        public void ReturnNewBasket_IfItDoesNotExistsButRequested()
        {
            var prob = CreateTestProbe();
            var product = GetProduct();

            var dataSource = new Mock<IProductsDataSource>();
            dataSource
                .Setup(x => x.GetAll())
                .Returns(new List<Product> {product});

            var productActor = Sys.ActorOf(ProductActor.Props(dataSource.Object));
            var target = Sys.ActorOf(BasketActor.Props(1, productActor));

            target.Tell(new GetBasket(1, 1), prob);

            var actual = prob.ExpectMsg<BasketFound>(x => x.CorrelationId == 1 && x.CustomerId == 1);

            actual.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task ManageItemsInBasket_IfAddOrRemoveMessageReceived()
        {
            var prob = CreateTestProbe();
            var product = GetProduct();

            var dataSource = new Mock<IProductsDataSource>();
            dataSource
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { product });

            var productActor = Sys.ActorOf(ProductActor.Props(dataSource.Object));
            var target = Sys.ActorOf(BasketActor.Props(1, productActor));

            target.Tell(new AddItemToTheBasket(
                correlationId: 1,
                customerId: 1,
                amount: 1,
                productId: 1), prob);

            prob.ExpectMsg<ItemAddedToTheBasket>(x => x.CorrelationId == 1 && x.CustomerId == 1);

            var actual = await target.Ask<BasketFound>(new GetBasket(2, 1));
            actual.Items.Should().ContainKey(1);
            actual.Items[1].Should().Be(1);

            target.Tell(new RemoveItemFromTheBasket(3, 1, 1));
            await target.Ask<BasketFound>(new GetBasket(2, 1));
            actual.Items.Should().BeEmpty();
        }

        [Fact]
        public void ReturnProductNotFound_IfProductIsNotInTheBasket()
        {
            var prob = CreateTestProbe();
            var product = GetProduct();

            var dataSource = new Mock<IProductsDataSource>();
            dataSource
                .Setup(x => x.GetAll())
                .Returns(new List<Product> { product });

            var productActor = Sys.ActorOf(ProductActor.Props(dataSource.Object));
            var target = Sys.ActorOf(BasketActor.Props(1, productActor));

            target.Tell(new RemoveItemFromTheBasket(1, 1, 333), prob);

            prob.ExpectMsg<ProductNotFound>(x => x.CorrelationId == 1 && x.CustomerId == 1);
        }
    }
}
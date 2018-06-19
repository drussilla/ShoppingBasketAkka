using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit;
using FluentAssertions;
using Moq;
using ShoppingBasket.Domains.ProductDomain;
using ShoppingBasket.Domains.ProductDomain.Messages;
using ShoppingBasket.Domains.ProductDomain.Responses;
using Xunit;

namespace Domains.Tests
{
    public class ProductActorShould : TestKit
    {
        private Product GetProduct() => new Product
        {
            Id = 1,
            Title = "test",
            AmountInStock = 1,
            ItemPrice = 999
        };

        [Fact]
        public void ReturnProductNotFound_WhenProductIsNotInDataSet()
        {
            //arrange

            var dataSource = new Mock<IProductsDataSource>();
            dataSource.Setup(x => x.GetAll()).Returns(new List<Product>
            {
                GetProduct()
            });

            var prob = CreateTestProbe();
            var target = Sys.ActorOf(ProductActor.Props(dataSource.Object));

            target.Tell(new GetProduct(1, 777), prob.Ref);
            prob.ExpectMsg<ProductNotFound>(x => x.CorrelationId == 1);

            target.Tell(new TakeProductFromStock(2, 777, 100), prob.Ref);
            prob.ExpectMsg<ProductNotFound>(x => x.CorrelationId == 2);
        }

        [Fact]
        public void ReturnAllProducts_OnGetAllProductsMessage()
        {
            //arrange
            var expectedProduct = GetProduct();
            var dataSource = new Mock<IProductsDataSource>();
            dataSource.Setup(x => x.GetAll()).Returns(new List<Product>
            {
                expectedProduct
            });

            var prob = CreateTestProbe();
            var target = Sys.ActorOf(ProductActor.Props(dataSource.Object));

            target.Tell(new GetAllProducts(1), prob.Ref);
            var response = prob.ExpectMsg<AllProducts>(x => x.CorrelationId == 1);

            response.Products.Count.Should().Be(1);
            response.Products.Should().Contain(expectedProduct);
        }

        [Fact]
        public async Task ReturnProductTakenFromStock_WhenProductIsTakenFromStock()
        {
            //arrange
            var expectedProduct = GetProduct();
            var dataSource = new Mock<IProductsDataSource>();
            dataSource.Setup(x => x.GetAll()).Returns(new List<Product>
            {
                expectedProduct
            });

            var prob = CreateTestProbe();
            var target = Sys.ActorOf(ProductActor.Props(dataSource.Object));

            target.Tell(new TakeProductFromStock(1, productId: 1, amountToTakeFromStock: 1), prob.Ref);
            prob.ExpectMsg<ProductTakenFromStock>(x => x.CorrelationId == 1);

            var actual = await target.Ask<ProductFound>(new GetProduct(2, 1));
            actual.Product.AmountInStock.Should().Be(0);
        }

        [Fact]
        public void ReturnNotEnoughItemsInStock_WhenThereIsNotEnoughItemsInStock()
        {
            //arrange
            var expectedProduct = GetProduct();
            var dataSource = new Mock<IProductsDataSource>();
            dataSource.Setup(x => x.GetAll()).Returns(new List<Product>
            {
                expectedProduct
            });

            var prob = CreateTestProbe();
            var target = Sys.ActorOf(ProductActor.Props(dataSource.Object));

            target.Tell(new TakeProductFromStock(1, productId: 1, amountToTakeFromStock: 10), prob.Ref);
            var response = prob.ExpectMsg<NotEnoughItemsInStock>(x => x.CorrelationId == 1);

            response.ItemsAvailableInStock.Should().Be(1);
        }
    }
}

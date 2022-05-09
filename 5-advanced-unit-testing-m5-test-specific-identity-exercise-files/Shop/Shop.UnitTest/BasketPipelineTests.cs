using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class BasketPipelineTests
    {
        [Fact]
        public void SutCorrectlyConvertsToPipe()
        {
            CompositePipe<Basket> sut = new BasketPipeline();

            var visitors = sut
                .Cast<BasketVisitorPipe>()
                .Select(bvp => bvp.Visitor);

            var dv = Assert.IsAssignableFrom<VolumeDiscountVisitor>(visitors.First());
            Assert.Equal(500, dv.Threshold);
            Assert.Equal(.05m, dv.Rate);

            var vv = Assert.IsAssignableFrom<VatVisitor>(visitors.ElementAt(1));
            Assert.Equal(.25m, vv.Rate);

            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(visitors.Last());
        }



        [Fact]
        public void UseBasketPipelineOnExpensiveBasket()
        {
            // Fixture setup
            var basket = new Basket(
                new BasketItem("Chocolate", 50, 3),
                new BasketItem("Gruyère", 45.5m, 1),
                new BasketItem("Barolo", 250, 2));
            CompositePipe<Basket> pipeline = new BasketPipeline();
            // Exercise system
            var actual = pipeline.Pipe(basket);
            // Verify outcome
            var expected = new Basket(
                new BasketItem("Chocolate", 50, 3),
                new BasketItem("Gruyère", 45.5m, 1),
                new BasketItem("Barolo", 250, 2),
                new Discount(34.775m),
                new Vat(165.18125m),
                new BasketTotal(825.90625m));
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}

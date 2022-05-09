using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class Scenario
    {
        [Fact]
        public void UseVistorToCalculateSimpleTotal()
        {
            // Fixture setup
            var basket = new Basket(
                new BasketItem("Chocolate", 50, 2),
                new BasketItem("Rice", 30, 1),
                new BasketItem("Wine", 110, 1));
            // Exercise system
            var actual = basket.Accept(new BasketTotalVisitor());
            // Verify outcome
            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(actual);
            Assert.Equal(240, btv.Total);
            // Teardown
        }

        [Fact]
        public void AddTotalToSimpleBasket()
        {
            // Fixture setup
            var basket = new Basket(
                new BasketItem("Chocolate", 50, 2),
                new BasketItem("Rice", 30, 1),
                new BasketItem("Wine", 110, 1));
            var pipe = new CompositePipe<Basket>(
                new BasketVisitorPipe(
                    new BasketTotalVisitor()));
            // Exercise system
            var actual = pipe.Pipe(basket);
            // Verify outcome
            Assert.Equal(
                240m,
                actual.OfType<BasketTotal>().Single());
            // Teardown
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
            var printout = actual.Accept(new BasketStringWriterVisitor()).ToString();
            // Verify outcome
            var bi1 = Assert.IsAssignableFrom<BasketItem>(
                actual.ElementAt(0));
            Assert.Equal("Chocolate", bi1.Name);
            Assert.Equal(50, bi1.UnitPrice);
            Assert.Equal(3, bi1.Quantity);

            var bi2 = Assert.IsAssignableFrom<BasketItem>(
                actual.ElementAt(1));
            Assert.Equal("Gruyère", bi2.Name);
            Assert.Equal(45.5m, bi2.UnitPrice);
            Assert.Equal(1, bi2.Quantity);

            var bi3 = Assert.IsAssignableFrom<BasketItem>(
                actual.ElementAt(2));
            Assert.Equal("Barolo", bi3.Name);
            Assert.Equal(250, bi3.UnitPrice);
            Assert.Equal(2, bi3.Quantity);

            var d = Assert.IsAssignableFrom<Discount>(actual.ElementAt(3));
            Assert.Equal(34.775m, d);

            var v = Assert.IsAssignableFrom<Vat>(actual.ElementAt(4));
            Assert.Equal(165.18125m, v);

            var bt = Assert.IsAssignableFrom<BasketTotal>(actual.ElementAt(5));
            Assert.Equal(825.90625m, bt);

            Assert.Equal(
                @"
Chocolate:         3     50,00    150,00
Gruyère:           1     45,50     45,50
Barolo:            2    250,00    500,00
Discount:                         -34,78
VAT:                              165,18
Total:                            825,91",
                printout);
            // Teardown
        }

        [Fact]
        public void UseBasketPipelineOnInexpensiveBasket()
        {
            // Fixture setup
            var basket = new Basket(
                new BasketItem("Water", 8, 2));
            CompositePipe<Basket> pipeline = new BasketPipeline();
            // Exercise system
            var actual = pipeline.Pipe(basket);
            var printout = actual.Accept(new BasketStringWriterVisitor()).ToString();
            // Verify outcome
            var bi = Assert.IsAssignableFrom<BasketItem>(
                actual.ElementAt(0));
            Assert.Equal("Water", bi.Name);
            Assert.Equal(8, bi.UnitPrice);
            Assert.Equal(2, bi.Quantity);

            var v = Assert.IsAssignableFrom<Vat>(actual.ElementAt(1));
            Assert.Equal(4m, v);

            var bt = Assert.IsAssignableFrom<BasketTotal>(actual.ElementAt(2));
            Assert.Equal(20m, bt);

            Assert.Equal(
                @"
Water:             2      8,00     16,00
VAT:                                4,00
Total:                             20,00",
                printout);
            // Teardown
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;
using Xunit.Extensions;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class BasketTotalVisitorTests
    {
        [Fact]
        public void SutIsBasketVisitor()
        {
            var sut = new BasketTotalVisitor();
            Assert.IsAssignableFrom<IBasketVisitor>(sut);
        }

        [Fact]
        public void InitialTotalIsCorrect()
        {
            var sut = new BasketTotalVisitor();
            decimal actual = sut.Total;
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 2, 0)]
        [InlineData(2, 1, 0)]
        [InlineData(2, 1, 1)]
        public void VisitProductBasketElementReturnsCorrectResult(
            int unitPrice,
            int quantity,
            int initialTotal)
        {
            var sut = new BasketTotalVisitor(initialTotal);
            
            var productElement = 
                new BasketItem("Dummy name", unitPrice, quantity);
            var actual = sut.Visit(productElement);                
            
            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(actual);
            Assert.Equal(productElement.Total + initialTotal, btv.Total);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TotalIsCorrect(int expected)
        {
            var sut = new BasketTotalVisitor(expected);
            var actual = sut.Total;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 8)]
        public void VisitBasketTotalElementReturnsCorrectResult(
            int expected,
            int total)
        {
            var sut = new BasketTotalVisitor(expected);
            
            var actual = sut.Visit(new BasketTotal(total));

            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(actual);
            Assert.Equal(expected, btv.Total);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void VisitDiscountReturnsCorrectResult(
            int initialTotal,
            int discount)
        {
            var sut = new BasketTotalVisitor(initialTotal);

            var actual = sut.Visit(new Discount(discount));

            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(actual);
            Assert.Equal(initialTotal - discount, btv.Total);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void VisitVatReturnsCorrectResult(
            int initialTotal,
            int vatAmount)
        {
            var sut = new BasketTotalVisitor(initialTotal);

            var actual = sut.Visit(new Vat(vatAmount));

            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(actual);
            Assert.Equal(initialTotal + vatAmount, btv.Total);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void SutYieldsCorrectResult(int expected)
        {
            var sut = new BasketTotalVisitor(expected);

            var bt = Assert.IsAssignableFrom<BasketTotal>(sut.Single());
            Assert.Equal<decimal>(expected, bt);
            Assert.Equal<decimal>(
                expected,
                sut.OfType<BasketTotal>().Single());
        }
    }
}

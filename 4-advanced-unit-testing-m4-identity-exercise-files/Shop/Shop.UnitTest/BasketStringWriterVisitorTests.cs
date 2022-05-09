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
    public class BasketStringWriterVisitorTests
    {
        [Fact]
        public void SutIsBasketElement()
        {
            var sut = new BasketStringWriterVisitor();
            Assert.IsAssignableFrom<IBasketVisitor>(sut);
        }

        [Fact]
        public void DefaultToStringReturnsCorrectResult()
        {
            var sut = new BasketStringWriterVisitor();
            var actual = sut.ToString();
            Assert.Equal("", actual);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void ToStringReturnsCorrectResult(string expected)
        {
            var sut = new BasketStringWriterVisitor(expected);
            var actual = sut.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("My basket", "Wine", 130, 1)]
        [InlineData("other line", "Wine", 130, 1)]
        [InlineData("other line", "Chocolate", 130, 1)]
        [InlineData("other line", "Chocolate", 28, 1)]
        [InlineData("other line", "Chocolate", 28, 2)]
        public void VisitBasketItemReturnsCorrectResult(
            string initialBasketText,
            string productName,
            int unitPrice,
            int quantity)
        {
            var sut = new BasketStringWriterVisitor(initialBasketText);
            var actual = sut.Visit(
                new BasketItem(productName, unitPrice, quantity));
            Assert.Equal(
                string.Format(
                    "{0}{1}{2,-17}{3,3}{4,10:F}{5,10:F}",
                    initialBasketText,
                    Environment.NewLine,
                    productName + ":",
                    quantity,
                    unitPrice,
                    quantity * unitPrice),
                actual.ToString());
        }

        [Theory]
        [InlineData("My basket", 42)]
        [InlineData("other line", 42)]
        [InlineData("other line", 1337)]
        public void VisitBasketTotalReturnsCorrectResult(
            string initialBasketText,
            int total)
        {
            var sut = new BasketStringWriterVisitor(initialBasketText);
            var actual = sut.Visit(new BasketTotal(total));
            Assert.Equal(
                string.Format(
                    "{0}{1}Total:{2,34:F}",
                    initialBasketText,
                    Environment.NewLine,
                    total),
                actual.ToString());
        }

        [Theory]
        [InlineData("My basket", 42)]
        [InlineData("other line", 42)]
        [InlineData("other line", 2)]
        public void VisitDiscountReturnsCorrectResult(
            string initialBasketText,
            int discount)
        {
            var sut = new BasketStringWriterVisitor(initialBasketText);
            var actual = sut.Visit(new Discount(discount));
            Assert.Equal(
                string.Format(
                    "{0}{1}Discount:{2,31:F}",
                    initialBasketText,
                    Environment.NewLine,
                    -discount),
                actual.ToString());
        }

        [Theory]
        [InlineData("My basket", 42)]
        [InlineData("other line", 42)]
        [InlineData("other line", 2)]
        public void VisitVatReturnsCorrectResult(
            string initialBasketText,
            int vat)
        {
            var sut = new BasketStringWriterVisitor(initialBasketText);
            var actual = sut.Visit(new Vat(vat));
            Assert.Equal(
                string.Format(
                    "{0}{1}VAT:{2,36:F}",
                    initialBasketText,
                    Environment.NewLine,
                    vat),
                actual.ToString());
        }

        [Fact]
        public void SutYieldsNothing()
        {
            var sut = new BasketStringWriterVisitor();
            Assert.False(sut.Any());
            Assert.Empty(sut);
        }
    }
}

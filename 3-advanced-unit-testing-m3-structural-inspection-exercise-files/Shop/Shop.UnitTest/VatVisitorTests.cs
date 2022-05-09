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
    public class VatVisitorTests
    {
        [Fact]
        public void SutIsBasketVisitor()
        {
            var dummyRate = .25m;
            var sut = new VatVisitor(dummyRate);
            Assert.IsAssignableFrom<IBasketVisitor>(sut);
        }

        [Fact]
        public void DefaultAmountIsCorrect()
        {
            var dummyRate = .25m;
            var sut = new VatVisitor(dummyRate);
            decimal actual = sut.Amount;
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AmountIsCorrect(int expected)
        {
            var dummyRate = .25m;
            var sut = new VatVisitor(dummyRate, expected);

            var actual = sut.Amount;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RateIsCorrect(int expected)
        {
            var sut = new VatVisitor(expected);
            decimal actual = sut.Rate;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RateIsCorrectFromConstructorWithAmount(int expected)
        {
            var dummyAmount = 1;
            var sut = new VatVisitor(expected, dummyAmount);

            var actual = sut.Rate;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(2, 1, 1, 1)]
        [InlineData(2, 2, 1, 1)]
        [InlineData(2, 2, 2, 1)]
        [InlineData(2, 2, 2, 2)]
        public void VisitBasketItemReturnsCorrectResult(
            int rate,
            int amount,
            int unitPrice,
            int quantity)
        {
            var sut = new VatVisitor(rate, amount);
            var basketItem =
                new BasketItem("Dummy", unitPrice, quantity);

            var actual = sut.Visit(basketItem);                

            var v = Assert.IsAssignableFrom<VatVisitor>(actual);
            Assert.Equal(
                amount + basketItem.Total * rate,
                v.Amount);
            Assert.Equal(rate, v.Rate);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void VisitBasketTotalReturnsCorrectResult(
            int rate,
            int amount)
        {
            var sut = new VatVisitor(rate, amount);

            var actual = sut.Visit(new BasketTotal(42));

            var v = Assert.IsAssignableFrom<VatVisitor>(actual);
            Assert.Equal(rate, v.Rate);
            Assert.Equal(amount, v.Amount);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 2, 2)]
        public void VisitDiscountReturnsCorrectResult(
            int rate,
            int amount,
            int discountAmount)
        {
            var sut = new VatVisitor(rate, amount);

            var actual = sut.Visit(new Discount(discountAmount));

            var v = Assert.IsAssignableFrom<VatVisitor>(actual);
            Assert.Equal(rate, v.Rate);
            Assert.Equal(
                amount - discountAmount * rate,
                v.Amount);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void VisitVatReturnsCorrectResult(
            int rate,
            int amount)
        {
            var sut = new VatVisitor(rate, amount);

            var actual = sut.Visit(new Vat(.25m));

            var v = Assert.IsAssignableFrom<VatVisitor>(actual);
            Assert.Equal(rate, v.Rate);
            Assert.Equal(amount, v.Amount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void SutYieldsCorrectResult(int expected)
        {
            var dummyRate = 0.2m;
            var sut = new VatVisitor(dummyRate, expected);

            var v = Assert.IsAssignableFrom<Vat>(sut.Single());
            Assert.Equal(expected, v.Amount);
            Assert.Equal(expected, sut.OfType<Vat>().Single().Amount);
        }
    }
}

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
    public class VolumeDiscountVisitorTests
    {
        [Fact]
        public void SutIsBasketVisitor()
        {
            var dummyThreshold = 100;
            var dummyRate = .05m;
            var sut = new VolumeDiscountVisitor(dummyThreshold, dummyRate);
            Assert.IsAssignableFrom<IBasketVisitor>(sut);
        }

        [Fact]
        public void DefaultAmountIsCorrect()
        {
            var dummyThreshold = 100;
            var dummyRate = .05m;
            var sut = new VolumeDiscountVisitor(dummyThreshold, dummyRate);

            decimal actual = sut.Subtotal;

            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void SubtotalIsCorrect(int expected)
        {
            var dummyThreshold = 100;
            var dummyRate = .05m;
            var sut = new VolumeDiscountVisitor(
                dummyThreshold, dummyRate, expected);

            var actual = sut.Subtotal;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ThresholdIsCorrect(int expected)
        {
            var dummyRate = .05m;
            var sut = new VolumeDiscountVisitor(expected, dummyRate);

            decimal actual = sut.Threshold;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ThresholdIsCorrectFromConstructorWithSubtotal(int expected)
        {
            var dummyRate = .05m;
            var dummySubtotal = 4.2m;
            var sut = new VolumeDiscountVisitor(
                expected, dummyRate, dummySubtotal);

            var actual = sut.Threshold;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RateIsCorrect(int expected)
        {
            var dummyThreshold = 100;
            var sut = new VolumeDiscountVisitor(dummyThreshold, expected);

            decimal actual = sut.Rate;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RateIsCorrectFromConstructorWithSubtotal(int expected)
        {
            var dummyThreshold = 200;
            var dummySubtotal = 300;
            var sut = new VolumeDiscountVisitor(
                dummyThreshold, expected, dummySubtotal);

            var actual = sut.Rate;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 1)]
        [InlineData(2, 1, 1, 1, 1)]
        [InlineData(2, 2, 1, 1, 1)]
        [InlineData(2, 2, 2, 1, 1)]
        [InlineData(2, 2, 2, 2, 1)]
        [InlineData(2, 2, 2, 2, 2)]
        public void VisitBasketItemReturnsCorrectResult(
            int threshold,
            int rate,
            int subtotal,
            int unitPrice,
            int quantity)
        {
            var sut = new VolumeDiscountVisitor(threshold, rate, subtotal);
            
            var basketItem =
                new BasketItem("Dummy", unitPrice, quantity);
            var actual = sut.Visit(basketItem);

            var vd = Assert.IsAssignableFrom<VolumeDiscountVisitor>(actual);
            Assert.Equal(threshold, vd.Threshold);
            Assert.Equal(rate, vd.Rate);
            Assert.Equal(subtotal + basketItem.Total, vd.Subtotal);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 2, 2)]
        public void VisitBasketTotalReturnsCorrectResult(
            int threshold,
            int rate,
            int subtotal)
        {
            var sut = new VolumeDiscountVisitor(threshold, rate, subtotal);

            var actual = sut.Visit(new BasketTotal(42));

            var vd = Assert.IsAssignableFrom<VolumeDiscountVisitor>(actual);
            Assert.Equal(threshold, vd.Threshold);
            Assert.Equal(rate, vd.Rate);
            Assert.Equal(subtotal, vd.Subtotal);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 2, 2)]
        public void VisitDiscountReturnsCorrectResult(
            int threshold,
            int rate,
            int subtotal)
        {
            var sut = new VolumeDiscountVisitor(threshold, rate, subtotal);

            var actual = sut.Visit(new Discount(1337));

            var vd = Assert.IsAssignableFrom<VolumeDiscountVisitor>(actual);
            Assert.Equal(threshold, vd.Threshold);
            Assert.Equal(rate, vd.Rate);
            Assert.Equal(subtotal, vd.Subtotal);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 2, 2)]
        public void VisitVatReturnsCorrectResult(
            int threshold,
            int rate,
            int subtotal)
        {
            var sut = new VolumeDiscountVisitor(threshold, rate, subtotal);

            var actual = sut.Visit(new Vat(1337));

            var vd = Assert.IsAssignableFrom<VolumeDiscountVisitor>(actual);
            Assert.Equal(threshold, vd.Threshold);
            Assert.Equal(rate, vd.Rate);
            Assert.Equal(subtotal, vd.Subtotal);
        }

        [Theory]
        [InlineData(2, 1)]
        public void SutYieldsCorrectResultWhenSubtotalIsBelowThreshold(
            int threshold,
            int subtotal)
        {
            var dummyRate = .03m;
            var sut =
                new VolumeDiscountVisitor(threshold, dummyRate, subtotal);

            Assert.False(sut.Any());
            Assert.Empty(sut);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 1, 3)]
        [InlineData(2, 2, 3)]
        public void SutYieldsCorrectResultWhenSubtotalIsAboveThreshold(
            int threshold,
            int rate,
            int subtotal)
        {
            var sut = new VolumeDiscountVisitor(threshold, rate, subtotal);

            var d = Assert.IsAssignableFrom<Discount>(sut.Single());
            Assert.Equal<decimal>(subtotal * rate, d);
            Assert.Equal<decimal>(
                subtotal * rate,
                sut.OfType<Discount>().Single());
        }
    }
}

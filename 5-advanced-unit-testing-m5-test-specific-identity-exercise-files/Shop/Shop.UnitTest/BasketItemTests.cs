using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;
using Xunit.Extensions;
using Moq;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class BasketItemTests
    {
        [Fact]
        public void SutIsBasketElement()
        {
            var dummyName = "Dummy product";
            var dummyUnitPrice = 1m;
            var dummyQantity = 1;
            var sut = new BasketItem(dummyName, dummyUnitPrice, dummyQantity);
            Assert.IsAssignableFrom<IBasketElement>(sut);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void NameIsCorrect(string expected)
        {
            var dummyQuantity = 1;
            var dummyUnitPrice = 1m;
            var sut = new BasketItem(expected, dummyUnitPrice, dummyQuantity);

            string actual = sut.Name;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void QuantityIsCorrect(int expected)
        {
            var dummyName = "Dummy product";
            var dummyUnitPrice = 1m;
            var sut = new BasketItem(dummyName, dummyUnitPrice, expected);

            int actual = sut.Quantity;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void UnitPriceIsCorrect(int expected)
        {
            var dummyName = "Dummy product";
            var dummyQuantity = 1;
            var sut = new BasketItem(dummyName, expected, dummyQuantity);

            decimal actual = sut.UnitPrice;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AcceptReturnsCorrectResponse()
        {
            var r = new MockRepository(MockBehavior.Default)
            {
                DefaultValue = DefaultValue.Mock
            };
            var expected = r.Create<IBasketVisitor>().Object;
            var sut = new BasketItem("Dummy product", 1, 1);

            var visitorStub = r.Create<IBasketVisitor>();
            visitorStub.Setup(v => v.Visit(sut)).Returns(expected);
            IBasketVisitor actual = sut.Accept(visitorStub.Object);

            Assert.Same(expected, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void TotalIsCorrect(int unitPrice, int quantity)
        {
            var sut = new BasketItem("Dummy product", unitPrice, quantity);
            
            decimal actual = sut.Total;

            decimal expected = unitPrice * quantity;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", 1, 1, 1, 1, true)]
        [InlineData("foo", "", 1, 1, 1, 1, false)]
        [InlineData("", "bar", 1, 1, 1, 1, false)]
        [InlineData("foo", "foo", 1, 1, 1, 1, true)]
        [InlineData("foo", "foo", 2, 1, 1, 1, false)]
        [InlineData("foo", "foo", 2, 2, 1, 1, true)]
        [InlineData("foo", "foo", 2, 2, 2, 1, false)]
        [InlineData("foo", "foo", 2, 2, 2, 2, true)]
        public void EqualsReturnsCorrectResult(
            string sutName,
            string otherName,
            int sutUnitPrice,
            int otherUnitPrice,
            int sutQuantity,
            int otherQuantity,
            bool expected)
        {
            var sut = new BasketItem(sutName, sutUnitPrice, sutQuantity);
            var actual = sut.Equals(
                new BasketItem(otherName, otherUnitPrice, otherQuantity));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData(typeof(object))]
        public void EqualsAnyOtherObjectReturnsFalse(
            object other)
        {
            var sut = new BasketItem("Dummy", 1, 1);
            var actual = sut.Equals(other);
            Assert.False(actual, "Equals should return false.");
        }

        [Theory]
        [InlineData("", 1, 1)]
        [InlineData("Foo", 1, 1)]
        [InlineData("Foo", 2.9, 1)]
        [InlineData("Foo", 2.9, 2)]
        public void GetHashCodeReturnsCorrectResult(
            string name,
            double unitPriceD,
            int quantity)
        {
            var unitPrice = (decimal)unitPriceD;
            var sut = new BasketItem(name, unitPrice, quantity);

            var actual = sut.GetHashCode();

            var expected =
                name.GetHashCode() ^ 
                unitPrice.GetHashCode() ^
                quantity.GetHashCode();
            Assert.Equal(expected, actual);
        }
    }
}

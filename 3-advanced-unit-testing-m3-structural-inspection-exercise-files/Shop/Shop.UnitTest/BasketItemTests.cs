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
    }
}

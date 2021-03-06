using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;
using Moq;
using Xunit.Extensions;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class DiscountTests
    {
        [Fact]
        public void SutIsBasketElement()
        {
            var sut = new Discount();
            Assert.IsAssignableFrom<IBasketElement>(sut);
        }

        [Fact]
        public void AcceptReturnsCorrectResponse()
        {
            var r = new MockRepository(MockBehavior.Default)
            {
                DefaultValue = DefaultValue.Mock
            };
            var expected = r.Create<IBasketVisitor>().Object;
            var sut = new Discount();

            var visitorStub = r.Create<IBasketVisitor>();
            visitorStub.Setup(v => v.Visit(sut)).Returns(expected);
            var actual = sut.Accept(visitorStub.Object);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void InitialAmountIsCorrect()
        {
            var sut = new Discount();
            decimal actual = sut.Amount;
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AmountIsCorrect(int expected)
        {
            var sut = new Discount(expected);
            var actual = sut.Amount;
            Assert.Equal(expected, actual);
        }
    }
}

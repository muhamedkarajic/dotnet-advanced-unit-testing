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
    public class VatTests
    {
        [Fact]
        public void SutIsBasketElement()
        {
            var sut = new Vat();
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
            var sut = new Vat();

            var visitorStub = r.Create<IBasketVisitor>();
            visitorStub.Setup(v => v.Visit(sut)).Returns(expected);
            var actual = sut.Accept(visitorStub.Object);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void InitialAmountIsCorrect()
        {
            var sut = new Vat();
            decimal actual = sut;
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AmountIsCorrect(int expected)
        {
            var sut = new Vat(expected);
            decimal actual = sut;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(2, 1, false)]
        [InlineData(2, 2, true)]
        public void EqualsOtherSutReturnsCorrectResult(
            int sutAmount,
            int otherAmount,
            bool expected)
        {
            var sut = new Vat(sutAmount);
            var other = new Vat(otherAmount);

            var actual = sut.BothEquals(other);

            Assert.True(actual.All(expected.Equals));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(typeof(Version), false)]
        public void EqualsAnyOtherObjectReturnsCorrectResult(
            object other,
            bool expected)
        {
            var dummy = 24.8m;
            var sut = new Vat(dummy);

            var actual = sut.Equals(other);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1.1)]
        [InlineData(2.5)]
        public void GetHashCodeReturnsCorrectResult(
            double total)
        {
            var d = (decimal)total;
            var sut = new Vat(d);

            var actual = sut.GetHashCode();

            var expected = d.GetHashCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutIsEquatable()
        {
            var sut = new Vat(0);
            Assert.IsAssignableFrom<IEquatable<Vat>>(sut);
        }

        [Theory]
        [InlineData(1.1)]
        [InlineData(2.3)]
        public void SutCorrectlyConvertsToDecimal(
            double d)
        {
            var expected = (decimal)d;
            var sut = new Vat(expected);

            decimal actual = sut;

            Assert.Equal(expected, actual);
        }
    }
}

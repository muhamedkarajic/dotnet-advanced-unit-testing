using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;
using Moq;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class BasketTests
    {
        [Fact]
        public void SutIsIterator()
        {
            var sut = new Basket();
            Assert.IsAssignableFrom<IEnumerable<IBasketElement>>(sut);
        }

        [Fact]
        public void SutYieldsInjectedElements()
        {
            var expected = new[]
            {
                new Mock<IBasketElement>().Object,
                new Mock<IBasketElement>().Object,
                new Mock<IBasketElement>().Object
            };

            var sut = new Basket(expected);

            Assert.True(expected.SequenceEqual(sut));
            Assert.True(
                expected.Cast<object>().SequenceEqual(sut.OfType<object>()));
        }

        [Fact]
        public void SutIsBasketElement()
        {
            var sut = new Basket();
            Assert.IsAssignableFrom<IBasketElement>(sut);
        }

        [Fact]
        public void AcceptReturnsCorrectResult()
        {
            // Fixture setup
            var r = new MockRepository(MockBehavior.Default)
            {
                DefaultValue = DefaultValue.Mock 
            };
            var v1 = r.Create<IBasketVisitor>().Object;
            var v2 = r.Create<IBasketVisitor>().Object;
            var v3 = r.Create<IBasketVisitor>().Object;
            var e1Stub = r.Create<IBasketElement>();
            var e2Stub = r.Create<IBasketElement>();
            e1Stub.Setup(e => e.Accept(v1)).Returns(v2);
            e2Stub.Setup(e => e.Accept(v2)).Returns(v3);

            var sut = new Basket(e1Stub.Object, e2Stub.Object);
            // Exercise system
            var actual = sut.Accept(v1);
            // Verify outcome
            Assert.Same(v3, actual);
            // Teardown
        }
    }
}

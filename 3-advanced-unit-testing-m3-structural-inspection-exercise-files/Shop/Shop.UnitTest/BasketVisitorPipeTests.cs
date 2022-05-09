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
    public class BasketVisitorPipeTests
    {
        [Fact]
        public void SutIsBasketPipe()
        {
            var dummyVisitor = new Mock<IBasketVisitor>().Object;
            var sut = new BasketVisitorPipe(dummyVisitor);
            Assert.IsAssignableFrom<IPipe<Basket>>(sut);
        }

        [Fact]
        public void PipeReturnsCorrectResult()
        {
            // Fixture setup
            var r = new MockRepository(MockBehavior.Default)
            {
                DefaultValue = DefaultValue.Mock
            };

            var v1Stub = r.Create<IBasketVisitor>();
            var v2Stub = r.Create<IBasketVisitor>();
            var v3Stub = r.Create<IBasketVisitor>();

            var e1Stub = r.Create<IBasketElement>();
            var e2Stub = r.Create<IBasketElement>();
            e1Stub.Setup(e => e.Accept(v1Stub.Object)).Returns(v2Stub.Object);
            e2Stub.Setup(e => e.Accept(v2Stub.Object)).Returns(v3Stub.Object);

            var newElements = new[]
            {
                r.Create<IBasketElement>().Object,
                r.Create<IBasketElement>().Object,
                r.Create<IBasketElement>().Object,
            };
            v3Stub
                .Setup(v => v.GetEnumerator())
                .Returns(newElements.AsEnumerable().GetEnumerator());

            var sut = new BasketVisitorPipe(v1Stub.Object);
            // Exercise system
            var basket = new Basket(e1Stub.Object, e2Stub.Object);
            Basket actual = sut.Pipe(basket);
            // Verify outcome
            Assert.True(basket.Concat(newElements).SequenceEqual(actual));
            // Teardown
        }

        [Fact]
        public void VisitorIsCorrect()
        {
            var expected = new Mock<IBasketVisitor>
                { DefaultValue = DefaultValue.Mock }.Object;
            var sut = new BasketVisitorPipe(expected);

            IBasketVisitor actual = sut.Visitor;

            Assert.Same(expected, actual);
        }
    }
}

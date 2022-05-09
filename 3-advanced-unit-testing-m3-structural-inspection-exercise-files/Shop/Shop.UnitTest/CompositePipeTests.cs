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
    public abstract class CompositePipeTests<T>
    {
        [Fact]
        public void SutIsPipe()
        {
            var sut = new CompositePipe<T>();
            Assert.IsAssignableFrom<IPipe<T>>(sut);
        }

        [Fact]
        public void PipeReturnsCorrectResult()
        {
            // Fixture setup
            var v1 = this.CreateValue();
            var v2 = this.CreateValue();
            var v3 = this.CreateValue();
            var v4 = this.CreateValue();
            var p1 = new Mock<IPipe<T>>();
            var p2 = new Mock<IPipe<T>>();
            var p3 = new Mock<IPipe<T>>();
            p1.Setup(p => p.Pipe(v1)).Returns(v2);
            p2.Setup(p => p.Pipe(v2)).Returns(v3);
            p3.Setup(p => p.Pipe(v3)).Returns(v4);

            var sut = new CompositePipe<T>(p1.Object, p2.Object, p3.Object);

            // Exercise system
            var actual = sut.Pipe(v1);

            // Verify outcome
            Assert.Equal(v4, actual);

            // Teardown
        }

        [Fact]
        public void SutIsSequence()
        {
            var sut = new CompositePipe<T>();
            Assert.IsAssignableFrom<IEnumerable<IPipe<T>>>(sut);
        }

        [Fact]
        public void SutYieldsInjectedPipes()
        {
            var expected = new[]
            {
                new Mock<IPipe<T>>().Object,
                new Mock<IPipe<T>>().Object,
                new Mock<IPipe<T>>().Object,
            };
            var sut = new CompositePipe<T>(expected);
            Assert.True(expected.SequenceEqual(sut));
            Assert.True(
                expected.Cast<object>().SequenceEqual(sut.OfType<object>()));
        }

        public abstract T CreateValue();
    }

    public class CompositePipeTestsOfObject : CompositePipeTests<object>
    {
        public override object CreateValue()
        {
            return new object();
        }
    }

    public class CompositePipeTestsOfString : CompositePipeTests<string> 
    {
        public override string CreateValue()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public class CompositePipeTestsOfInt32 : CompositePipeTests<int> 
    {
        private readonly Random random;

        public CompositePipeTestsOfInt32()
        {
            this.random = new Random();
        }

        public override int CreateValue()
        {
            return this.random.Next();
        }
    }

    public class CompositePipeTestsOfGuid : CompositePipeTests<Guid>
    {
        public override Guid CreateValue()
        {
            return Guid.NewGuid();
        }
    }

    public class CompositePipeTestsOfVersion : CompositePipeTests<Version>
    {
        private readonly Random random;

        public CompositePipeTestsOfVersion()
        {
            this.random = new Random();
        }

        public override Version CreateValue()
        {
            return new Version(
                this.random.Next(),
                this.random.Next());
        }
    }
}

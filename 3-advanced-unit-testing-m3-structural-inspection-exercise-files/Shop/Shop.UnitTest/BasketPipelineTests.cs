using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.Shop;

namespace Ploeh.Samples.Shop.UnitTest
{
    public class BasketPipelineTests
    {
        [Fact]
        public void SutCorrectlyConvertsToPipe()
        {
            CompositePipe<Basket> sut = new BasketPipeline();

            var visitors = sut
                .Cast<BasketVisitorPipe>()
                .Select(bvp => bvp.Visitor);

            var dv = Assert.IsAssignableFrom<VolumeDiscountVisitor>(visitors.First());
            Assert.Equal(500, dv.Threshold);
            Assert.Equal(.05m, dv.Rate);

            var vv = Assert.IsAssignableFrom<VatVisitor>(visitors.ElementAt(1));
            Assert.Equal(.25m, vv.Rate);

            var btv = Assert.IsAssignableFrom<BasketTotalVisitor>(visitors.Last());
        }
    }
}

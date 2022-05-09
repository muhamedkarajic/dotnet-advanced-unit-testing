using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketPipeline
    {
        public static implicit operator CompositePipe<Basket>(
            BasketPipeline basketPipeline)
        {
            return new CompositePipe<Basket>(
                new BasketVisitorPipe(
                    new VolumeDiscountVisitor(500, .05m)),
                new BasketVisitorPipe(
                    new VatVisitor(.25m)),
                new BasketVisitorPipe(
                    new BasketTotalVisitor()));
        }
    }
}

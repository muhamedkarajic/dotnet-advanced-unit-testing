using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketVisitorPipe : IPipe<Basket>
    {
        private readonly IBasketVisitor visitor;

        public BasketVisitorPipe(IBasketVisitor visitor)
        {
            this.visitor = visitor;
        }

        public Basket Pipe(Basket basket)
        {
            var v = basket.Accept(this.visitor);
            return new Basket(basket.Concat(v).ToArray());
        }

        public IBasketVisitor Visitor
        {
            get { return this.visitor; }
        }
    }
}

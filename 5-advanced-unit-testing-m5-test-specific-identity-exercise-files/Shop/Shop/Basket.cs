using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class Basket : IEnumerable<IBasketElement>, IBasketElement
    {
        private readonly IEnumerable<IBasketElement> elements;

        public Basket(params IBasketElement[] elements)
        {
            this.elements = elements;
        }

        public IEnumerator<IBasketElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return this.elements.Aggregate(visitor, (v, e) => e.Accept(v));
        }
    }
}

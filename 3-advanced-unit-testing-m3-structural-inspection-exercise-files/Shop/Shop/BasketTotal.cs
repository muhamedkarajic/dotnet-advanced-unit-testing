using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketTotal : IBasketElement
    {
        private readonly decimal total;

        public BasketTotal()
        {
        }

        public BasketTotal(decimal total)
        {
            this.total = total;
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public decimal Total 
        {
            get { return this.total; }
        }
    }
}

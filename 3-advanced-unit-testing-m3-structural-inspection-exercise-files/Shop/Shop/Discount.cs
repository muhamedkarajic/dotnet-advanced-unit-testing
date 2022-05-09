using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class Discount : IBasketElement
    {
        private readonly decimal amount;

        public Discount()
        {
        }

        public Discount(decimal amount)
        {
            this.amount = amount;
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public decimal Amount 
        {
            get { return this.amount; }
        }
    }
}

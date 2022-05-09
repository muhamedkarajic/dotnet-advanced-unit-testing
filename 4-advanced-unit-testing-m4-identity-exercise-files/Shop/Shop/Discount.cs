using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public struct Discount : IBasketElement, IEquatable<Discount>
    {
        private readonly decimal amount;

        public Discount(decimal amount)
        {
            this.amount = amount;
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is Discount)
                return this.Equals((Discount)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.amount.GetHashCode();
        }

        public bool Equals(Discount other)
        {
            return this.amount == other.amount;
        }

        public static implicit operator decimal(Discount discount)
        {
            return discount.amount;
        }
    }
}

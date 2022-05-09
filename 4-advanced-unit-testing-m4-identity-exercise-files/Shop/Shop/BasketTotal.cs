using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public struct BasketTotal : IBasketElement, IEquatable<BasketTotal>
    {
        private readonly decimal total;

        public BasketTotal(decimal total)
        {
            this.total = total;
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is BasketTotal)
                return this.Equals((BasketTotal)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.total.GetHashCode();
        }

        public bool Equals(BasketTotal other)
        {
            return this.total == other.total;
        }

        public static implicit operator Decimal(BasketTotal basketTotal)
        {
            return basketTotal.total;
        }
    }
}

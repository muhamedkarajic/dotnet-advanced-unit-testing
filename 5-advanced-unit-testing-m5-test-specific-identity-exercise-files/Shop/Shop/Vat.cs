using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public struct Vat : IBasketElement, IEquatable<Vat>
    {
        private readonly decimal amount;

        public Vat(decimal amount)
        {
            this.amount = amount;
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vat)
                return this.Equals((Vat)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.amount.GetHashCode();
        }

        public bool Equals(Vat other)
        {
            return this.amount == other.amount;
        }

        public static implicit operator decimal(Vat vat)
        {
            return vat.amount;
        }
    }
}

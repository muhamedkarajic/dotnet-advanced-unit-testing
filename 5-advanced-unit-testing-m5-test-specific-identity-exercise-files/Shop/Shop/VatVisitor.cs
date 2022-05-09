using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class VatVisitor : IBasketVisitor
    {
        private readonly decimal rate;
        private readonly decimal amount;

        public VatVisitor(decimal rate)
            : this(rate, 0)
        {
        }

        public VatVisitor(decimal rate, decimal amount)
        {
            this.rate = rate;
            this.amount = amount;
        }

        public IBasketVisitor Visit(BasketItem basketItem)
        {
            return new VatVisitor(
                this.rate,
                this.amount + basketItem.Total * this.rate);
        }

        public IBasketVisitor Visit(BasketTotal basketTotal)
        {
            return this;
        }

        public IBasketVisitor Visit(Discount discount)
        {
            return new VatVisitor(
                this.rate,
                this.amount - discount * this.rate);
        }

        public IBasketVisitor Visit(Vat vat)
        {
            return this;
        }

        public IEnumerator<IBasketElement> GetEnumerator()
        {
            yield return new Vat(this.amount);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public decimal Amount
        {
            get { return this.amount; }
        }

        public decimal Rate
        {
            get { return this.rate; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class VolumeDiscountVisitor : IBasketVisitor
    {
        private readonly decimal threshold;
        private readonly decimal rate;
        private readonly decimal subtotal;

        public VolumeDiscountVisitor(decimal threshold, decimal rate)
        {
            this.threshold = threshold;
            this.rate = rate;
        }

        public VolumeDiscountVisitor(
            decimal threshold, 
            decimal rate, 
            decimal subtotal)
        {
            this.threshold = threshold;
            this.rate = rate;
            this.subtotal = subtotal;
        }

        public IBasketVisitor Visit(BasketItem basketItem)
        {
            return new VolumeDiscountVisitor(
                this.threshold, 
                this.rate,
                this.subtotal + basketItem.Total);
        }

        public IBasketVisitor Visit(BasketTotal basketTotal)
        {
            return this;
        }

        public IBasketVisitor Visit(Discount discount)
        {
            return this;
        }

        public IBasketVisitor Visit(Vat vat)
        {
            return this;
        }

        public IEnumerator<IBasketElement> GetEnumerator()
        {
            if (this.threshold <= this.subtotal)
                yield return new Discount(this.subtotal * this.rate);
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public decimal Subtotal
        {
            get { return this.subtotal; }
        }

        public decimal Threshold
        {
            get { return this.threshold; }
        }

        public decimal Rate
        {
            get { return this.rate; }
        }
    }
}

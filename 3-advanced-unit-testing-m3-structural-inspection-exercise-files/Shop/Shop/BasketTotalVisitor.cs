using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketTotalVisitor : IBasketVisitor
    {
        private readonly decimal total;

        public BasketTotalVisitor()
        {            
        }

        public BasketTotalVisitor(decimal total)
        {
            this.total = total;
        }

        public IBasketVisitor Visit(BasketItem basketItem)
        {
            return new BasketTotalVisitor(basketItem.Total + this.total);
        }

        public IBasketVisitor Visit(BasketTotal basketTotal)
        {
            return this;
        }

        public IBasketVisitor Visit(Discount discount)
        {
            return new BasketTotalVisitor(this.Total - discount.Amount);
        }

        public IBasketVisitor Visit(Vat vat)
        {
            return new BasketTotalVisitor(this.total + vat.Amount);
        }

        public decimal Total 
        {
            get { return this.total; }
        }

        public IEnumerator<IBasketElement> GetEnumerator()
        {
            yield return new BasketTotal(this.total);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

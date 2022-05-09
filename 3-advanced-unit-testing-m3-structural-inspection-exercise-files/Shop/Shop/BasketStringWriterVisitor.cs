using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketStringWriterVisitor : IBasketVisitor
    {
        private readonly string basketText;

        public BasketStringWriterVisitor()
            : this("")
        {
        }

        public BasketStringWriterVisitor(string basketText)
        {
            this.basketText = basketText;
        }

        public IBasketVisitor Visit(BasketItem basketItem)
        {
            return new BasketStringWriterVisitor(
                string.Format(
                    "{0}{1}{2,-17}{3,3}{4,10:F}{5,10:F}",
                    this.basketText,
                    Environment.NewLine,
                    basketItem.Name + ":",
                    basketItem.Quantity,
                    basketItem.UnitPrice,
                    basketItem.Total));
        }

        public IBasketVisitor Visit(BasketTotal basketTotal)
        {
            return new BasketStringWriterVisitor(
                string.Format(
                    "{0}{1}Total:{2,34:F}",
                    this.basketText,
                    Environment.NewLine,
                    basketTotal.Total));
        }

        public IBasketVisitor Visit(Discount discount)
        {
            return new BasketStringWriterVisitor(
                string.Format(
                    "{0}{1}Discount:{2,31:F}",
                    this.basketText,
                    Environment.NewLine,
                    -discount.Amount));
        }

        public IBasketVisitor Visit(Vat vat)
        {
            return new BasketStringWriterVisitor(
                string.Format(
                    "{0}{1}VAT:{2,36:F}",
                    this.basketText,
                    Environment.NewLine,
                    vat.Amount));
        }

        public IEnumerator<IBasketElement> GetEnumerator()
        {
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return this.basketText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class BasketItem : IBasketElement
    {
        private readonly string name;
        private readonly decimal unitPrice;
        private readonly int quantity;

        public BasketItem(
            string name,
            decimal unitPrice,
            int quantity)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.quantity = quantity;
        }

        public string Name
        {
            get { return this.name; }
        }

        public int Quantity
        {
            get { return this.quantity; }
        }

        public decimal UnitPrice
        {
            get { return this.unitPrice; }
        }

        public IBasketVisitor Accept(IBasketVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public decimal Total
        {
            get { return this.quantity * this.unitPrice; }
        }
    }
}

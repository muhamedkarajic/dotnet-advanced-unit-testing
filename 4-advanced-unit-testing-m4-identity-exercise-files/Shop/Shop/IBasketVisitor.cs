using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public interface IBasketVisitor : IEnumerable<IBasketElement>
    {
        IBasketVisitor Visit(BasketItem basketItem);

        IBasketVisitor Visit(BasketTotal basketTotal);

        IBasketVisitor Visit(Discount discount);

        IBasketVisitor Visit(Vat vat);
    }
}

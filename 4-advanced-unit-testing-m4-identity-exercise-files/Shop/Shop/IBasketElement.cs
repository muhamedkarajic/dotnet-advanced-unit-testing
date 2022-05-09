using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public interface IBasketElement
    {
        IBasketVisitor Accept(IBasketVisitor visitor);
    }
}

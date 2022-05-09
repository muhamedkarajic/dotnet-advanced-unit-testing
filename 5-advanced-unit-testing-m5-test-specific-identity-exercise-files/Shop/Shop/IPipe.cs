using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public interface IPipe<T>
    {
        T Pipe(T item);
    }
}

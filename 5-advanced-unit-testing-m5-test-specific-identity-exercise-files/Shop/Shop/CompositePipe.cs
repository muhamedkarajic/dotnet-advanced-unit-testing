using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Shop
{
    public class CompositePipe<T> : IPipe<T>, IEnumerable<IPipe<T>>
    {
        private readonly IEnumerable<IPipe<T>> pipes;

        public CompositePipe(params IPipe<T>[] pipes)
        {
            this.pipes = pipes;
        }

        public T Pipe(T item)
        {
            return this.pipes.Aggregate(item, (x, p) => p.Pipe(x));
        }

        public IEnumerator<IPipe<T>> GetEnumerator()
        {
            return this.pipes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

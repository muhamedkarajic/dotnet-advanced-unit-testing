using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.Shop.UnitTest
{
    public static class EqualityEnvy
    {
        public static IEnumerable<bool> BothEquals<T>(this T sut, T other)
            where T : IEquatable<T>
        {
            yield return sut.Equals((object)other);
            yield return sut.Equals(other);
        }
    }
}

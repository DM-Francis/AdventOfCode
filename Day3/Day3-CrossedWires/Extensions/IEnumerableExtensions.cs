using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day3_CrossedWires.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}

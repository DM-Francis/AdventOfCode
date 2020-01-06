using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day7_AmplificationCircuit.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}

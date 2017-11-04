using System.Collections.Generic;

namespace Nanomsg2.Sharp
{
    internal static class ReceiverExtensionMethods
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach (var x in values)
            {
                collection.Add(x);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, params T[] values)
        {
            foreach (var x in values)
            {
                collection.Add(x);
            }
        }
    }
}

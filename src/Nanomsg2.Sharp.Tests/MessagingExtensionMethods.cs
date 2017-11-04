using System.Collections.Generic;
using System.Linq;

namespace Nanomsg2.Sharp
{
    internal static class MessagingExtensionMethods
    {
        public static IEnumerable<byte> ToBytes(this string s)
            => s.Select(x => (byte) x);

        public static string ReassembleString(this IEnumerable<byte> bytes)
            => bytes.Aggregate(string.Empty, (g, x) => g + (char) x);
    }
}

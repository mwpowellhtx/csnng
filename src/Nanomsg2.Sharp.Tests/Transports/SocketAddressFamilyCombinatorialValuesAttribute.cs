using System;
using System.Linq;

namespace Nanomsg2.Sharp.Transports
{
    using Xunit;
    using static SocketAddressFamily;

    [Obsolete("TODO: TBD: I'm not sure why this is not working.")]
    public class SocketAddressFamilyCombinatorialValuesAttribute : CombinatorialValuesAttribute
    {
        private static readonly object[] Families;

        static SocketAddressFamilyCombinatorialValuesAttribute()
        {
            Families = new object[]
            {
                (ushort) Unspecified, (ushort) InProcess,
                (ushort) InterProcess, (ushort) IPv4,
                (ushort) IPv6, (ushort) ZeroTier,
                (ushort) 0xfeed, (ushort) 0xfade,
            };
        }

        public SocketAddressFamilyCombinatorialValuesAttribute()
            : base(Families.ToArray())
        {
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Transports
{
    using Xunit;
    using Xunit.Abstractions;
    using static Marshal;

    public class SocketAddressStructureTests : TransportTestBase
    {
        public SocketAddressStructureTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        private static void VerifySockAddr(ref SOCKADDR sa, ushort family)
        {
            Assert.Equal(family, sa.Family);
            Assert.Equal(family, sa.Unspec.Family);
            Assert.Equal(family, sa.InProc.Family);
            Assert.Equal(family, sa.InterProc.Family);
            Assert.Equal(family, sa.IPv4.Family);
            Assert.Equal(family, sa.IPv6.Family);
            Assert.Equal(family, sa.ZeroTier.Family);
        }

        [Theory]
        [InlineData(0x1)]
        [InlineData(0x10)]
        [InlineData(0x100)]
        public void ThatFamilyChangesImpactsAllViews(ushort family)
        {
            var sa = new SOCKADDR();
            VerifySockAddr(ref sa, 0);
            sa.Family = family;
            VerifySockAddr(ref sa, family);
        }

        [Theory]
        [InlineData(typeof(ushort), 2)]
        [InlineData(typeof(uint), 4)]
        [InlineData(typeof(ulong), 8)]
        [InlineData(typeof(SOCKADDR), 130)]
        [InlineData(typeof(SOCKADDR_Unspecified), 2)]
        [InlineData(typeof(SOCKADDR_Path), 130)]
        [InlineData(typeof(SOCKADDR_IPv4), 8)]
        [InlineData(typeof(SOCKADDR_IPv6), 20)]
        [InlineData(typeof(SOCKADDR_IPv6_Addr), 16)]
        [InlineData(typeof(SOCKADDR_IPv6_Addr16), 16)]
        [InlineData(typeof(SOCKADDR_IPv6_Addr32), 16)]
        [InlineData(typeof(SOCKADDR_IPv6_Addr64), 16)]
        [InlineData(typeof(SOCKADDR_ZeroTier), 20)]
        public void ThatStructSizeIsCorrect(Type structType, int expectedSz)
        {
            Out.WriteLine("Verifying structure size of {0}, expecting {1} bytes.", structType, expectedSz);
            Assert.NotNull(structType);
            Assert.True(expectedSz > 0, $"{nameof(expectedSz)} should be greater than zero (0).");
            /* Not only does this vet the Expected Size, but it also has the interesting side effect of
             * detecting any obscurities that have the potential to block safe (or unsafe fixed) usage
             * when we need it. */
            Assert.Equal(expectedSz, SizeOf(structType));
        }
    }
}

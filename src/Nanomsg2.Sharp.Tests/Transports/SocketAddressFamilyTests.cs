namespace Nanomsg2.Sharp.Transports
{
    using Xunit;
    using Xunit.Abstractions;
    using static SocketAddressFamily;

    public class SocketAddressFamilyTests : TransportTestBase
    {
        public SocketAddressFamilyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Theory]
        [InlineData(Unspecified, 0)]
        [InlineData(InProcess, 1)]
        [InlineData(InterProcess, 2)]
        [InlineData(IPv4, 3)]
        [InlineData(IPv6, 4)]
        [InlineData(ZeroTier, 5)]
        public void ThatEnumValuesAreCorrect(SocketAddressFamily family, ushort expected)
        {
            Assert.Equal(expected, (ushort) family);
        }
    }
}

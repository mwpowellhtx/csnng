namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Xunit.Abstractions;

    public class IPv6BusTests : BusTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv6;

        public IPv6BusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Xunit.Abstractions;

    public class IPv4BusTests : BusTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4BusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

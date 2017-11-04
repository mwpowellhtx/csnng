namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using Xunit.Abstractions;

    public class IPv6ReqRepTests : ReqRepTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv6;

        public IPv6ReqRepTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

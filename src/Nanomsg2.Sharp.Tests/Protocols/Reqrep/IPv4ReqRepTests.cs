namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using Xunit.Abstractions;

    public class IPv4ReqRepTests : ReqRepTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4ReqRepTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

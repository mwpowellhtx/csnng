namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using Xunit.Abstractions;

    public class InterProcessReqRepTests : ReqRepTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InterProcess;

        public InterProcessReqRepTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

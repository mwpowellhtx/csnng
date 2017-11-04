namespace Nanomsg2.Sharp.Protocols.Pubsub
{
    using Xunit.Abstractions;

    public class IPv6PubSubTests : PubSubTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv6;

        public IPv6PubSubTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

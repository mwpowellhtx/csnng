namespace Nanomsg2.Sharp.Protocols.Pubsub
{
    using Xunit.Abstractions;

    public class IPv4PubSubTests : PubSubTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4PubSubTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

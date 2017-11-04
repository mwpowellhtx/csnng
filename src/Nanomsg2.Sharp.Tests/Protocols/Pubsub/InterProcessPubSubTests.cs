namespace Nanomsg2.Sharp.Protocols.Pubsub
{
    using Xunit.Abstractions;

    public class InterProcessPubSubTests : PubSubTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InProcess;

        public InterProcessPubSubTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

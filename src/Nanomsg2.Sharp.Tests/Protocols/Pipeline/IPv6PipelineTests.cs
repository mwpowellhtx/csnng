namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Xunit.Abstractions;

    public class IPv6PipelineTests : PipelineTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv6;

        public IPv6PipelineTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

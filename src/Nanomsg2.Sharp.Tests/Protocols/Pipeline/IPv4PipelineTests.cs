namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Xunit.Abstractions;

    public class IPv4PipelineTests : PipelineTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4PipelineTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

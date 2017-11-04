namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Xunit.Abstractions;

    public class InterProcessPipelineTests : PipelineTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InterProcess;

        public InterProcessPipelineTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

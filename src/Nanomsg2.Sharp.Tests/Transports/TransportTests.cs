namespace Nanomsg2.Sharp.Transports
{
    using Xunit.Abstractions;

    public abstract class TransportTestBase
    {
        protected ITestOutputHelper Out { get; }

        protected TransportTestBase(ITestOutputHelper @out)
        {
            Out = @out;
        }
    }
}

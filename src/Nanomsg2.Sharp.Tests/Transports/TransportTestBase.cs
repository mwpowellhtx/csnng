namespace Nanomsg2.Sharp.Transports
{
    using Xunit.Abstractions;

    public abstract class TransportTestBase : TestFixtureBase
    {
        protected TransportTestBase(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

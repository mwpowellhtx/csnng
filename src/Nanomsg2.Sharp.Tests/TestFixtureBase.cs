namespace Nanomsg2.Sharp
{
    using Xunit.Abstractions;

    public abstract class TestFixtureBase
    {
        protected ITestOutputHelper Out { get; }

        protected TestFixtureBase(ITestOutputHelper @out)
        {
            Out = @out;
        }
    }
}

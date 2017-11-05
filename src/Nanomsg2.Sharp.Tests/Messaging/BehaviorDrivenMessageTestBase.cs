namespace Nanomsg2.Sharp.Messaging
{
    using Xunit.Abstractions;

    public abstract class BehaviorDrivenMessageTestBase : BehaviorDrivenTestFixtureBase
    {
        protected BehaviorDrivenMessageTestBase(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

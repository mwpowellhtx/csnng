namespace Nanomsg2.Sharp.Protocols.Survey
{
    using Xunit.Abstractions;

    public class IPv6SurveyTests : SurveyTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv6;

        public IPv6SurveyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

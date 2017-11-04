namespace Nanomsg2.Sharp.Protocols.Survey
{
    using Xunit.Abstractions;

    public class IPv4SurveyTests : SurveyTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4SurveyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

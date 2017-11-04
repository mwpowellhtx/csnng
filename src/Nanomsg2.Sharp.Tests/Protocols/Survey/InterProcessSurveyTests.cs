namespace Nanomsg2.Sharp.Protocols.Survey
{
    using Xunit.Abstractions;

    public class InterProcessSurveyTests : SurveyTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InterProcess;

        public InterProcessSurveyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

using System.Xml.Serialization;
using Xunit;

namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Xunit.Abstractions;

    public class InterProcessBusTests : BusTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InterProcess;

        public InterProcessBusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_default_socket_correct()
        {
            base.That_default_socket_correct();
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out()
        {
            base.That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out();
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_Bus1_delivers_message_to_both_Bus2_and_Bus3()
        {
            base.That_Bus1_delivers_message_to_both_Bus2_and_Bus3();
        }
    }
}

using System;
using System.Linq;

namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static Exceptions;
    using static ErrorCode;
    using O = Options;

    public class BusTests : ProtocolTestBase<LatestBusSocket>
    {
        private const string TestAddr = "inproc://test";
        private const string NinetyNineBits = "99bits";
        private const string OnThe = "onthe";

        private Message _message;
        private LatestBusSocket[] _sockets;

        public BusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        ~BusTests()
        {
            _message?.Dispose();
            _sockets?[0]?.Dispose();
            _sockets?[1]?.Dispose();
            _sockets?[2]?.Dispose();
        }

        private void Facilitate(string title, Action action)
        {
            Given($"three {typeof(LatestBusSocket).FullName} instances", () =>
            {
                _sockets = new[] {CreateOne(), CreateOne(), CreateOne()};

                _sockets[0].Listen(TestAddr);
                _sockets[1].Dial(TestAddr);
                _sockets[2].Dial(TestAddr);

                var recvTimeout = FromMilliseconds(50d);

                _sockets[0].Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);
                _sockets[1].Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);
                _sockets[2].Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);

                Section("messages can be delivered", () =>
                {
                    var m = _message = CreateMessage();

                    var b1 = _sockets[0];
                    var b2 = _sockets[1];
                    var b3 = _sockets[2];

                    Section("receive times out", () =>
                    {
                        Throws<NanoException>(() => b1.TryReceive(m), ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                        Throws<NanoException>(() => b2.TryReceive(m), ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                        Throws<NanoException>(() => b3.TryReceive(m), ex => ex.ErrorNumber.ToErrorCode() == TimedOut);

                        Section(title, action);
                    });
                });
            });

            _sockets.ToList().ForEach(s => s.Dispose());
        }

        [Fact]
        public void That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out()
        {
            Facilitate("Bus2 delivers message to Bus1, Bus3 times out", () =>
            {
                var m = _message;

                var b1 = _sockets[0];
                var b2 = _sockets[1];
                var b3 = _sockets[2];

                m.Body.Append(NinetyNineBits);
                b2.Send(m);
                Assert.True(b1.TryReceive(m));
                Assert.Equal(NinetyNineBits.Select(x => (byte) x), m.Body.Get());
                m.Dispose();

                m = CreateMessage();
                Throws<NanoException>(() => b3.TryReceive(m), ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
            });
        }

        [Fact]
        public void That_Bus1_delivers_message_to_both_Bus2_and_Bus3()
        {
            Facilitate("Bus1 delivers message to both Bus2 and Bus3", () =>
            {
                var m = _message;

                var b1 = _sockets[0];
                var b2 = _sockets[1];
                var b3 = _sockets[2];

                m.Body.Append(OnThe);
                b1.Send(m);

                b2.TryReceive(m);
                Assert.Equal(OnThe.Select(x => (byte) x), m.Body.Get());

                using (var m2 = CreateMessage())
                {
                    b3.TryReceive(m2);
                    Assert.Equal(OnThe.Select(x => (byte) x), m2.Body.Get());
                    Assert.False(m2.SameAs(m));
                }
            });
        }
    }
}

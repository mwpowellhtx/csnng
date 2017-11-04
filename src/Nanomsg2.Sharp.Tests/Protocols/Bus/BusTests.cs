using System;
using System.Linq;

namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static ErrorCode;
    using O = Options;

    public class BusTests : ProtocolTestBase
    {
        private const string NinetyNineBits = "99bits";
        private const string OnThe = "onthe";

        private Message _message;
        private LatestBusSocket[] _sockets;

        private ScenarioDelegate Facilitate { get; }

        public BusTests(ITestOutputHelper @out)
            : base(@out)
        {
            Facilitate = action =>
            {
                try
                {
                    var addr = TestAddr;

                    Given($"three {typeof(LatestBusSocket).FullName} instances", () =>
                    {
                        _sockets = new[]
                        {
                            CreateOne<LatestBusSocket>(),
                            CreateOne<LatestBusSocket>(),
                            CreateOne<LatestBusSocket>()
                        };

                        _sockets[0].Listen(addr);
                        _sockets[1].Dial(addr);
                        _sockets[2].Dial(addr);

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
                                Assert.Throws<NanoException>(() => b1.TryReceive(m))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                                Assert.Throws<NanoException>(() => b2.TryReceive(m))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                                Assert.Throws<NanoException>(() => b3.TryReceive(m))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);

                                action();
                            });
                        });
                    });
                }
                finally
                {
                    DisposeAll(_sockets.ToArray<IDisposable>());
                }
            };
        }

        [Fact]
        public virtual void That_default_socket_correct()
        {
            Given_default_socket<LatestBusSocket>();
        }

        [Fact]
        public virtual void That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out()
        {
            Facilitate(() =>
            {
                Section("Bus2 delivers message to Bus1, Bus3 times out", () =>
                {
                    var m = _message;

                    var b1 = _sockets[0];
                    var b2 = _sockets[1];
                    var b3 = _sockets[2];

                    m.Body.Append(NinetyNineBits);
                    b2.Send(m);
                    Assert.True(b1.TryReceive(m));
                    Assert.Equal(NinetyNineBits.ToBytes(), m.Body.Get());
                    m.Dispose();

                    m = CreateMessage();

                    Assert.Throws<NanoException>(() => b3.TryReceive(m))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                });
            });
        }

        [Fact]
        public virtual void That_Bus1_delivers_message_to_both_Bus2_and_Bus3()
        {
            Facilitate(() =>
            {
                Section("Bus1 delivers message to both Bus2 and Bus3", () =>
                {
                    var m = _message;

                    var b1 = _sockets[0];
                    var b2 = _sockets[1];
                    var b3 = _sockets[2];

                    m.Body.Append(OnThe);
                    b1.Send(m);

                    Assert.True(b2.TryReceive(m));
                    Assert.Equal(OnThe.ToBytes(), m.Body.Get());

                    using (var m2 = CreateMessage())
                    {
                        Assert.True(b3.TryReceive(m2));
                        Assert.Equal(OnThe.ToBytes(), m2.Body.Get());
                        Assert.False(m2.SameAs(m));
                    }
                });
            });
        }

        ~BusTests()
        {
            _message?.Dispose();
            _sockets?[0]?.Dispose();
            _sockets?[1]?.Dispose();
            _sockets?[2]?.Dispose();
        }
    }
}

using System;

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

        public BusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact]
        public virtual void That_default_socket_correct()
        {
            Given_default_socket<LatestBusSocket>();
        }

        private delegate void LinkedSocketCallback(LatestBusSocket bus1, LatestBusSocket bus2, LatestBusSocket bus3);

        private void Given_Bus_sockets(LinkedSocketCallback callback)
        {
            var addr = TestAddr;

            Given($"three '{typeof(LatestBusSocket).FullName}' instances", () =>
            {
                LatestBusSocket bus1 = null;
                LatestBusSocket bus2 = null;
                LatestBusSocket bus3 = null;

                try
                {
                    bus1 = CreateOne<LatestBusSocket>();
                    bus2 = CreateOne<LatestBusSocket>();
                    bus3 = CreateOne<LatestBusSocket>();

                    bus1.Listen(addr);
                    bus2.Dial(addr);
                    bus3.Dial(addr);

                    var recvTimeout = FromMilliseconds(50d);

                    bus1.Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);
                    bus2.Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);
                    bus3.Options.SetDuration(O.RecvTimeoutDuration, recvTimeout);

                    Section("messages can be delivered", () =>
                    {
                        Section("receive times out", () =>
                        {
                            Message m = null;
                            try
                            {
                                m = CreateMessage();

                                var n = m;

                                Assert.Throws<NanoException>(() => bus1.TryReceive(n))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                                Assert.Throws<NanoException>(() => bus2.TryReceive(n))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                                Assert.Throws<NanoException>(() => bus3.TryReceive(n))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                            }
                            finally
                            {
                                m?.Dispose();
                            }
                        });

                        callback(bus1, bus2, bus3);
                    });
                }
                finally
                {
                    DisposeAll(bus1, bus2, bus3);
                }
            });
        }

        [Fact]
        public virtual void That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out()
        {
            Given_Bus_sockets((bus1, bus2, bus3) =>
            {
                Section("Bus2 delivers message to Bus1, Bus3 times out", () =>
                {
                    using (var m = CreateMessage())
                    {
                        m.Body.Append(NinetyNineBits);
                        bus2.Send(m);
                        Assert.True(bus1.TryReceive(m));
                        Assert.Equal(NinetyNineBits.ToBytes(), m.Body.Get());

                        m.Clear();

                        var n = m;

                        Assert.Throws<NanoException>(() => bus3.TryReceive(n))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                    }
                });
            });
        }

        [Fact]
        public virtual void That_Bus1_delivers_message_to_both_Bus2_and_Bus3()
        {
            Given_Bus_sockets((bus1, bus2, bus3) =>
            {
                Section("Bus1 delivers message to both Bus2 and Bus3", () =>
                {
                    using (var m = CreateMessage())
                    {
                        m.Body.Append(OnThe);
                        bus1.Send(m);

                        Assert.True(bus2.TryReceive(m));
                        Assert.Equal(OnThe.ToBytes(), m.Body.Get());

                        using (var m2 = CreateMessage())
                        {
                            Assert.True(bus3.TryReceive(m2));
                            Assert.Equal(OnThe.ToBytes(), m2.Body.Get());
                            Assert.False(m2.SameAs(m));
                        }
                    }
                });
            });
        }
    }
}

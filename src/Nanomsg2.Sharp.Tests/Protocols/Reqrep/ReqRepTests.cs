using System;

namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static ErrorCode;
    using O = Options;

    public class ReqRepTests : ProtocolTestBase
    {
        private const string TestAddr = "inproc://test";
        private const string Abc = "abc";
        private const string Def = "def";
        private const string Ping = "ping";
        private const string Pong = "pong";


        public ReqRepTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact]
        public void That_Requestor_can_set_resend_time_option()
        {
            Given_default_socket<LatestReqSocket>(s =>
            {
                Section("can set resend time option", () =>
                {
                    s.Options.SetDuration(O.ReqResendDuration, FromMilliseconds(1d));
                });
            });
        }

        [Fact]
        public void That_Requestor_receive_without_send_fails()
        {
            Given_default_socket<LatestReqSocket>(s =>
            {
                Section("receive without send fails", () =>
                {
                    Message m = null;
                    try
                    {
                        m = CreateMessage();
                        Assert.Throws<NanoException>(() => s.TryReceive(m))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == State);
                    }
                    finally
                    {
                        DisposeAll(m);
                    }
                });
            });
        }

        [Fact]
        public void That_Replier_cannot_set_Requestor_resend_time_option()
        {
            Given_default_socket<LatestRepSocket>(s =>
            {
                Section("cannot set Requestor resend time option", () =>
                {
                    var timeout = FromMilliseconds(1d);
                    Assert.Throws<NanoException>(() => s.Options.SetDuration(O.ReqResendDuration, timeout))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NotSupported);
                });
            });
        }

        [Fact]
        public void That_Replier_send_without_receive_fails()
        {
            Given_default_socket<LatestRepSocket>(s =>
            {
                Section("send without receive fails", () =>
                {
                    Message m = null;
                    try
                    {
                        m = CreateMessage();
                        Assert.Throws<NanoException>(() => s.Send(m))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == State);
                    }
                    finally
                    {
                        DisposeAll(m);
                    }
                });
            });
        }

        private delegate void LinkedSocketsCallback(LatestReqSocket req, LatestRepSocket rep);

        private void Given_two_fresh_sockets(LinkedSocketsCallback callback)
        {
            Section("given two fresh sockets", () =>
            {
                LatestReqSocket req = null;
                LatestRepSocket rep = null;
                try
                {
                    req = CreateOne<LatestReqSocket>();
                    rep = CreateOne<LatestRepSocket>();

                    callback(req, rep);
                }
                finally
                {
                    DisposeAll(req, rep);
                }
            });
        }

        private void We_can_create_linked_sockets(LinkedSocketsCallback callback)
        {
            Given_two_fresh_sockets((req, rep) =>
            {
                Section("we can create linked sockets", () =>
                {
                    rep.Listen(TestAddr);
                    req.Dial(TestAddr);

                    callback(req, rep);
                });
            });
        }

        [Fact]
        public void The_sockets_can_exchange_messages()
        {
            We_can_create_linked_sockets((req, rep) =>
            {
                Section("the sockets can exchange messages", () =>
                {
                    Message ping = null;
                    Message pong = null;

                    try
                    {
                        ping = CreateMessage();
                        pong = CreateMessage();

                        ping.Body.Append(Ping);
                        req.Send(ping);
                        Assert.True(rep.TryReceive(pong));
                        Assert.Equal(Ping.ToBytes(), pong.Body.Get());

                        pong.Clear();
                        pong.Body.Append(Pong);
                        rep.Send(pong);
                        Assert.True(req.TryReceive(ping));
                        Assert.Equal(Pong.ToBytes(), ping.Body.Get());
                    }
                    finally
                    {
                        DisposeAll(ping, pong);
                    }
                });
            });
        }

        [Fact]
        public void Request_cancellation_works()
        {
            Given_two_fresh_sockets((req, rep) =>
            {
                req.Options.SetDuration(O.ReqResendDuration, FromMilliseconds(100d));
                req.Options.SetInt32(O.SendBuf, 16);

                rep.Listen(TestAddr);
                req.Dial(TestAddr);

                Message abc = null;
                Message def = null;
                Message cmd = null;

                try
                {
                    abc = CreateMessage();
                    def = CreateMessage();
                    cmd = CreateMessage();

                    abc.Body.Append(Abc);
                    def.Body.Append(Def);

                    req.Send(abc);
                    req.Send(def);

                    Assert.True(rep.TryReceive(cmd));
                    Assert.True(cmd.HasOne);

                    rep.Send(cmd);
                    Assert.True(rep.TryReceive(cmd));
                    rep.Send(cmd);
                    Assert.True(req.TryReceive(cmd));

                    Assert.Equal(Def.ToBytes(), cmd.Body.Get());
                }
                finally
                {
                    DisposeAll(abc, def, cmd);
                }

                Section("request cancellation works", () =>
                {
                });
            });
        }
    }
}

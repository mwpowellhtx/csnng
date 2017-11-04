using System;

namespace Nanomsg2.Sharp.Protocols.Pubsub
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static ErrorCode;
    using O = Options;

    public class PubSubTests : ProtocolTestBase
    {
        private const string TestAddr = "inproc://test";
        private const string Abc = "abc";
        private const string Empty = "";

        // ReSharper disable InconsistentNaming
        private static class Topics
        {
            public const string Some = "/some/";
            public const string Some_like_it_hot = "/some/like/it/hot";
            public const string Some_day_some_how = "/some/day/some/how";
            public const string Some_do_not_like_it = "some/do/not/like/it";
            public const string Some_like_it_raw = "/some/like/it/raw";
            public const string Somewhere_over_the_rainbow = "/somewhere/over/the/rainbow";
            public const string Something_aint_quite_right = "/something/aint/quite/right";
        }
        // ReSharper enable InconsistentNaming

        public PubSubTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact]
        public void That_default_Subscriber_Socket_correct()
        {
            That_default_Receiver_Socket_correct<LatestSubSocket>();
        }

        [Fact]
        public void That_default_Publisher_Socket_correct()
        {
            That_default_Sender_Socket_correct<LatestPubSocket>();

            // These are some additional properties that are true for Publisher Sockets.
            Given_default_socket<LatestPubSocket>(s =>
            {
                Section("Publisher cannot subscribe", () =>
                {
                    Assert.Throws<NanoException>(() => s.Options.SetString(O.SubSubscribe, Empty))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NotSupported);
                });

                Section("Publisher cannot unsubscribe", () =>
                {
                    // Goes without saying, but we should test it just the same.
                    Assert.Throws<NanoException>(() => s.Options.SetString(O.SubUnsubscribe, Empty))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NotSupported);
                });
            });
        }

        private delegate void LinkedSocketCallback(LatestPubSocket pub, LatestSubSocket sub);

        private void We_can_create_linked_sockets(LinkedSocketCallback callback)
        {
            Assert.NotNull(callback);

            Given_fresh_slate("can create linked sockets", () =>
            {
                LatestPubSocket pub = null;
                LatestSubSocket sub = null;

                try
                {
                    pub = CreateOne<LatestPubSocket>();
                    sub = CreateOne<LatestSubSocket>();

                    // Serves other paths of unit testing.
                    sub.Options.SetDuration(O.RecvTimeoutDuration, FromMilliseconds(90d));

                    // TODO: TBD: yes, in this case, Subscriber is the "server" listening for publishers.
                    // TODO: TBD: I would think this might be the other way round... or could be... does not necessarily HAVE to be...
                    sub.Listen(TestAddr);
                    pub.Dial(TestAddr);

                    callback(pub, sub);
                }
                finally
                {
                    DisposeAll(pub, sub);
                }
            });
        }

        private void Subscriber_can_subscribe(LinkedSocketCallback callback)
        {
            Assert.NotNull(callback);

            We_can_create_linked_sockets((pub, sub) =>
            {
                Section("Subscriber can subscribe", () =>
                {
                    sub.Options.SetString(O.SubSubscribe, Abc);
                    sub.Options.SetString(O.SubSubscribe, Empty);

                    callback(pub, sub);
                });
            });
        }

        [Fact]
        public void Unsubscribe_works()
        {
            Subscriber_can_subscribe((pub, sub) =>
            {
                Section("Unsubscribe works", () =>
                {
                    sub.Options.SetString(O.SubUnsubscribe, Abc);
                    sub.Options.SetString(O.SubUnsubscribe, Empty);
                });
            });
        }

        private void Subscriber_can_receive_from_Publisher(LinkedSocketCallback callback)
        {
            We_can_create_linked_sockets((pub, sub) =>
            {
                Section("Subscriber can receive from publisher", () =>
                {
                    sub.Options.SetString(O.SubSubscribe, Topics.Some);

                    callback(pub, sub);
                });
            });
        }

        [Fact]
        public void Valid_topics_are_received_by_Subscriber()
        {
            Subscriber_can_receive_from_Publisher((pub, sub) =>
            {
                Section("Valid topics are received by Subscriber", () =>
                {
                    foreach (var x in new[]
                    {
                        Topics.Some_like_it_hot
                        , Topics.Some_day_some_how
                    })
                    {
                        Section($"Topic: '{x}'", () =>
                        {
                            using (var m = CreateMessage())
                            {
                                m.Body.Append(x);
                                pub.Send(m);
                                Assert.True(sub.TryReceive(m));
                                Assert.Equal(x.ToBytes(), m.Body.Get());
                            }
                        });
                    }
                });
            });
        }

        [Fact]
        public void Invalid_topics_are_not_received_by_Subscriber()
        {
            Subscriber_can_receive_from_Publisher((pub, sub) =>
            {
                Section("Invalid topics are not received by Subscriber", () =>
                {
                    foreach (var x in new[]
                    {
                        Topics.Somewhere_over_the_rainbow
                        , Topics.Something_aint_quite_right
                    })
                    {
                        Section($"Topic: '{x}'", () =>
                        {
                            Message m = null;
                            try
                            {
                                m = CreateMessage();
                                m.Body.Append(x);
                                pub.Send(m);
                                Assert.Throws<NanoException>(() => sub.TryReceive(m))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                            }
                            finally
                            {
                                DisposeAll(m);
                            }
                        });
                    }
                });
            });
        }

        [Fact]
        private void Subscribers_without_subscriptions_do_not_receive()
        {
            We_can_create_linked_sockets((pub, sub) =>
            {
                Section("Subscribers without subscriptions do not receive", () =>
                {
                    var m = CreateMessage();

                    m.Body.Append(Topics.Some_do_not_like_it);
                    pub.Send(m);

                    Assert.Throws<NanoException>(() => sub.TryReceive(m))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                });
            });
        }

        [Fact]
        private void Subscribers_in_raw_receive()
        {
            We_can_create_linked_sockets((pub, sub) =>
            {
                Section("Subscribers in raw receive", () =>
                {
                    var m = CreateMessage();

                    sub.Options.SetInt32(O.Raw, 1);

                    m.Body.Append(Topics.Some_like_it_raw);
                    pub.Send(m);
                    Assert.True(sub.TryReceive(m));

                    Assert.Equal(Topics.Some_like_it_raw.ToBytes(), m.Body.Get());
                });
            });
        }
    }
}

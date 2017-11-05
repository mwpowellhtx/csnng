//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

namespace Nanomsg2.Sharp.Messaging
{
    using Protocols.Pair;
    using Xunit;
    using Xunit.Abstractions;
    using static SocketAddressFamily;
    using static ErrorCode;
    using O = Options;

    public class MessagePipeTests : BehaviorDrivenMessageTestBase
    {
        private string TestAddr
        {
            get
            {
                var addr = InProcess.BuildAddress<MessagePipeTests>();
                Out.WriteLine($"Testing using address '{addr}'");
                return addr;
            }
        }

        /// <summary>
        /// "a message was sent"
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const string a_message_was_sent = "a message was sent";

        public MessagePipeTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        private delegate void MessageCallback(Message message);

        private void Verify_without_sockets(MessageCallback callback)
        {
            Section($"{typeof(Message).FullName} with no {typeof(Socket).FullName} does not have one", () =>
            {
                var m = CreateMessage();
                callback(m);
            });
        }

        [Fact]
        public void Default_message_without_sockets_yields_pipe_not_having_one()
        {
            Verify_without_sockets(m =>
            {
                using (CreateMessagePipe(m, false))
                {
                }
            });
        }

        private delegate void MessagePipeCallback(Socket s, Message m, MessagePipe p);

        private const int ExpectedRecvBuf = 3;

        private void Prepare_pipe_given_sockets_and_messages(MessagePipeCallback callback)
        {
            var messagePipeType = typeof(MessagePipe).FullName;
            var socketType = typeof(Socket).FullName;
            var messageType = typeof(Message).FullName;

            var addr = TestAddr;

            Section($"'{messagePipeType}' constructs properly given two "
                    + $"'{socketType}' instances and '{messageType}' passed", () =>
            {
                Message m1 = null;
                Message m2 = null;

                Socket s1 = null;
                Socket s2 = null;

                try
                {
                    m1 = CreateMessage();
                    m2 = CreateMessage();

                    s1 = CreateOne<LatestPairSocket>();
                    s2 = CreateOne<LatestPairSocket>();

                    // Plant this seed for internal verification during subsequent steps.
                    s2.Options.SetInt32(O.RecvBuf, ExpectedRecvBuf);

                    // Some degree of cross-cutting concerns is unavoidable.
                    s1.Listen(addr);
                    s2.Dial(addr);

                    m1.Body.Append(a_message_was_sent);
                    s1.Send(m1);
                    s2.TryReceive(m2);
                    Assert.Equal(a_message_was_sent.ToBytes(), m2.Body.Get());

                    using (var p = CreateMessagePipe(m2))
                    {
                        callback(s2, m2, p);
                    }
                }
                finally
                {
                    DisposeAll(s1, s2, m1, m2);
                }
            });
        }

        [Fact]
        public void Resets_properly()
        {
            Prepare_pipe_given_sockets_and_messages((s, m, p) =>
            {
                Section("resets properly", () =>
                {
                    Section("still has one after reset", () =>
                    {
                        p.Reset();

                        // And it still HasOne after Reset.
                        Assert.True(p.HasOne);
                    });

                    Section("whose planted seed is correct", () =>
                    {
                        var actual = p.Options.GetInt32(O.RecvBuf);
                        Assert.Equal(ExpectedRecvBuf, actual);
                    });
                });
            });
        }

        [Fact]
        public void Operates_after_Disposing_originating_Message()
        {
            Prepare_pipe_given_sockets_and_messages((s, m, p) =>
            {
                Section("operates after Disposing the originating Message", () =>
                {
                    m.Dispose();

                    Section("whose planted seed is still correct", () =>
                    {
                        var actual = p.Options.GetInt32(O.RecvBuf);
                        Assert.Equal(ExpectedRecvBuf, actual);
                    });
                });
            });
        }

        [Fact]
        public void Get_option_throws_NoEnt_after_Disposing_Socket_channel()
        {
            var socketType = typeof(Socket).FullName;

            Prepare_pipe_given_sockets_and_messages((s, m, p) =>
            {
                Section($"get option throws '{NoEntry}' after Disposing '{socketType}' channel", () =>
                {
                    s.Dispose();

                    Assert.Throws<NanoException>(() => p.Options.GetInt32(O.RecvBuf))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NoEntry);
                });
            });
        }

        [Fact]
        public void Can_be_associated_with_another_Message()
        {
            Prepare_pipe_given_sockets_and_messages((s, m, p) =>
            {
                Section("can associate another message with the Socket channel", () =>
                {
                    using (var m2 = CreateMessage())
                    {
                        p.Set(m2);

                        var actual = p.Options.GetInt32(O.RecvBuf);

                        Assert.Equal(ExpectedRecvBuf, actual);
                    }
                });
            });
        }
    }
}

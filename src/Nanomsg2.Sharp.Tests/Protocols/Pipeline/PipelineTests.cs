using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static Thread;
    using static ErrorCode;
    using O = Options;

    public class PipelineTests : ProtocolTestBase
    {
        private const string TestAddr = "inproc://test";
        private const string Hello = "hello";
        private const string Abc = "abc";
        private const string Def = "def";

        public PipelineTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact]
        public void That_default_Pull_Socket_correct()
        {
            const string bufferTypeName = "System.Collections.Generic.IEnumerable<System.Byte>";
            var exceptionTypeName = typeof(InvalidOperationException).FullName;

            Given_default_socket<LatestPullSocket>(s =>
            {
                var bytes = new byte[0];
                var m = CreateMessage();

                Section($"does not support '{O.SendFd}'", () =>
                {
                    Assert.Throws<NanoException>(() => s.Options.GetInt32(O.SendFd))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NotSupported);
                });

                Section($"send '{m.GetType().FullName}' throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.Send(m));
                });

                Section($"send '{typeof(string).FullName}' throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.Send(string.Empty));
                });

                Section($"send '{bufferTypeName}' throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.Send(bytes));
                });

                Section($"send '{bufferTypeName}' with '{typeof(int).FullName}' 'count' throws '{exceptionTypeName}'",
                    () =>
                    {
                        Assert.Throws<InvalidOperationException>(() => s.Send(bytes, default(int)));
                    });

                Section($"send '{bufferTypeName}' with '{typeof(long).FullName}' 'count' throws '{exceptionTypeName}'",
                    () =>
                    {
                        Assert.Throws<InvalidOperationException>(() => s.Send(bytes, default(long)));
                    });
            });
        }

        [Fact]
        public void That_default_Push_Socket_correct()
        {
            var exceptionTypeName = typeof(InvalidOperationException).FullName;
            const string collectionTypeName = "System.Collections.Generic.ICollection<System.Byte>";

            Given_default_socket<LatestPushSocket>(s =>
            {
                Section($"does not support '{O.RecvFd}'", () =>
                {
                    Assert.Throws<NanoException>(() => s.Options.GetInt32(O.RecvFd))
                        .Matching(ex => ex.ErrorNumber.ToErrorCode() == NotSupported);
                });

                var m = CreateMessage();

                Section($"receiving '{m.GetType().FullName}' throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.ReceiveMessage());
                });

                var count = 0;

                Section($"receiving bytes throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.ReceiveBytes(ref count));
                });

                Section($"try receive '{collectionTypeName}' throws '{exceptionTypeName}'", () =>
                {
                    var bytes = new List<byte>();
                    Assert.Throws<InvalidOperationException>(() => s.TryReceive(bytes, ref count));
                });

                Section($"try receive '{m.GetType().FullName}' throws '{exceptionTypeName}'", () =>
                {
                    Assert.Throws<InvalidOperationException>(() => s.TryReceive(m));
                });
            });
        }

        [Fact]
        public void That_default_Pull_Socket_can_close()
        {
            That_socket_can_close<LatestPullSocket>();
        }

        private delegate void LinkedPushPullCallback(LatestPushSocket push, LatestPullSocket pull);

        private void We_can_create_linked_push_pull_sockets(LinkedPushPullCallback callback)
        {
            var pull = CreateOne<LatestPullSocket>();
            var push = CreateOne<LatestPushSocket>();

            pull.Listen(TestAddr);
            push.Dial(TestAddr);

            using (var what = CreateOne<LatestPullSocket>())
            {
                what.Dial(TestAddr);
            }

            Sleep(FromMilliseconds(20d));

            callback(push, pull);

            DisposeAll(pull, push);
        }

        [Fact]
        public void Push_can_send_Pull_can_receive()
        {
            var m = CreateMessage();

            We_can_create_linked_push_pull_sockets((push, pull) =>
            {
                m.Body.Append(Hello);
                push.Send(m);
                pull.TryReceive(m);
                Assert.Equal(Hello.ToBytes(), m.Body.Get());
            });
        }

        [Fact]
        public void Load_balancing_works()
        {
            var push = CreateOne<LatestPushSocket>();

            var pulls = new[]
            {
                CreateOne<LatestPullSocket>(),
                CreateOne<LatestPullSocket>(),
                CreateOne<LatestPullSocket>()
            };

            var pull1 = pulls[0];
            var pull2 = pulls[1];
            var pull3 = pulls[2];

            Action<Socket> setDefaultSendReceiveBufferSizes = s =>
            {
                const int defaultBufferSz = 4;
                s.Options.SetInt32(O.SendBuf, defaultBufferSz);
                s.Options.SetInt32(O.RecvBuf, defaultBufferSz);
            };

            var timeout = FromMilliseconds(100d);

            Action<Socket> setReceiveTimeoutDuration = s =>
            {
                s.Options.SetDuration(O.RecvTimeoutDuration, timeout);
            };

            ConfigureAll(setDefaultSendReceiveBufferSizes, push, pull1, pull2, pull3);
            ConfigureAll(setReceiveTimeoutDuration, pull1, pull2, pull3);

            push.Listen(TestAddr);
            pull1.Dial(TestAddr);
            pull2.Dial(TestAddr);
            pull3.Dial(TestAddr);
            pull3.Dispose();

            Sleep(timeout);

            var abc = CreateMessage();
            var def = CreateMessage();

            abc.Body.Append(Abc);
            def.Body.Append(Def);

            push.Send(abc);
            push.Send(def);

            pull1.TryReceive(abc);
            pull2.TryReceive(def);

            Assert.Equal(Abc.ToBytes(), abc.Body.Get());
            Assert.Equal(Def.ToBytes(), def.Body.Get());

            Assert.Throws<NanoException>(() => pull1.TryReceive(abc))
                .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);

            Assert.Throws<NanoException>(() => pull2.TryReceive(abc))
                .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);

            DisposeAll(push, pull1, pull3);
        }
    }
}

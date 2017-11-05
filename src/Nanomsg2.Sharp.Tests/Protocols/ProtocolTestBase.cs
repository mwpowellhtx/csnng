using System;
using System.Collections.Generic;
using System.Linq;

namespace Nanomsg2.Sharp.Protocols
{
    using Xunit;
    using Xunit.Abstractions;
    using static ErrorCode;
    using O = Options;
    using static SocketAddressFamily;

    public abstract class ProtocolTestBase : BehaviorDrivenTestFixtureBase
    {
        protected virtual SocketAddressFamily Family { get; } = InProcess;

        private SocketAddressFamily VerifyFamily(SocketAddressFamily family)
        {
            if (new[] {Unspecified, ZeroTier}.Contains(family))
            {
                throw new ArgumentException($"Family unsupported (for now): '{family}'", nameof(family));
            }
            Report($"Running protocol tests for address family '{family}'.");
            return family;
        }

        protected string TestAddr
        {
            get
            {
                var addr = VerifyFamily(Family).BuildAddress(GetType());
                Report($"Testing using address '{addr}'.");
                return addr;
            }
        }

        protected static void VerifyDefaultSocket<T>(T s)
            where T : Socket, new()
        {
            Assert.NotNull(s);
            Assert.True(s.HasOne);
            Assert.NotNull(s.Options);
            Assert.True(s.Options.HasOne);
        }

        protected ProtocolTestBase(ITestOutputHelper @out)
            : base(@out)
        {
        }

        protected virtual void Given_fresh_slate(string title, Action action)
        {
            Section(title, action);
        }

        protected virtual void Given_default_socket<T>()
            where T : Socket, new()
        {
            Given_default_socket<T>(s => { });
        }

        protected virtual void Given_default_socket<T>(Action<T> action)
            where T : Socket, new()
        {
            Section($"given default {typeof(T).FullName} socket", () =>
            {
                using (var s = new T())
                {
                    action(s);
                }
            });
        }

        protected virtual void That_socket_can_close<T>()
            where T : Socket, new()
        {
            Given_default_socket<T>(s =>
            {
                // ReSharper disable once ConvertClosureToMethodGroup
                Section($" '{typeof(T).FullName}' can {nameof(ISocket.Close)}", () =>
                {
                    s.Close();
                });
            });
        }

        protected static T CreateOne<T>()
            where T : Socket, new()
        {
            var s = new T();
            VerifyDefaultSocket(s);
            return s;
        }

        protected static void ConfigureAll(Action<Socket> action, params Socket[] sockets)
        {
            Assert.NotNull(action);
            sockets.ToList().ForEach(action);
        }

        protected static TimeSpan CreateMilliseconds(double value)
        {
            Assert.True(value > 0d || value.Equals(0d));
            return TimeSpan.FromMilliseconds(value);
        }

        protected virtual void That_default_Receiver_Socket_correct<T>()
            where T : Socket, new()
        {
            const string bufferTypeName = "System.Collections.Generic.IEnumerable<System.Byte>";
            var exceptionTypeName = typeof(InvalidOperationException).FullName;

            Given_default_socket<T>(s =>
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

        protected virtual void That_default_Sender_Socket_correct<T>()
            where T : Socket, new()
        {
            var exceptionTypeName = typeof(InvalidOperationException).FullName;
            const string collectionTypeName = "System.Collections.Generic.ICollection<System.Byte>";

            Given_default_socket<T>(s =>
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
    }
}

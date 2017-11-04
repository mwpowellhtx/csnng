using System;
using System.Linq;

namespace Nanomsg2.Sharp.Protocols
{
    using Xunit;
    using Xunit.Abstractions;

    public abstract class ProtocolTestBase : BehaviorDrivenTestFixtureBase

    {
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
    }
}

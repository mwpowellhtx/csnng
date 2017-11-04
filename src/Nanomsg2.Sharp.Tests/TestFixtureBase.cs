using System;
using System.Linq;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static Math;

    public abstract class TestFixtureBase
    {
        protected ITestOutputHelper Out { get; }

        protected TestFixtureBase(ITestOutputHelper @out)
        {
            Out = @out;
        }

        protected static void VerifyDefaultMessage(Message m)
        {
            Assert.NotNull(m);
            Assert.True(m.HasOne);
            Assert.True(m.Size == 0ul);
        }

        protected static Message CreateMessage()
        {
            var message = new Message();
            VerifyDefaultMessage(message);
            return message;
        }

        protected static void DisposeAll(params IDisposable[] items)
        {
            items.ToList().ForEach(d => d?.Dispose());
        }
    }

    internal static class PortExtensionMethods
    {
        private const int MinPort = 10000;
        private const int MaxPort = 10999;

        private static int? _port;

        private static int GetPort(int delta)
        {
            _port = _port ?? 0;
            _port += delta;
            // ReSharper disable once PossibleInvalidOperationException
            return (_port = Max(MinPort, Min(_port.Value, MaxPort))).Value;
        }

        public static string WithPort(this string addr, int delta = 1)
        {
            return $"{addr}:{GetPort(delta)}";
        }
    }
}

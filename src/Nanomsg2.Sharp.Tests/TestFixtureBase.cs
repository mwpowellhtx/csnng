using System;
using System.Linq;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;

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
}

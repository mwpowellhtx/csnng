using System;

namespace Nanomsg2.Sharp.Protocols
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class ProtocolTestBase<TSocket> : BehaviorDrivenTestFixtureBase
        where TSocket : Socket, new()

    {
        protected static void VerifyDefaultSocket(TSocket s)
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

        protected static TSocket CreateOne()
        {
            var s = new TSocket();
            VerifyDefaultSocket(s);
            return s;
        }

        protected static TimeSpan CreateMilliseconds(double value)
        {
            Assert.True(value > 0d || value.Equals(0d));
            return TimeSpan.FromMilliseconds(value);
        }

        [Fact]
        public void That_default_socket_correct()
        {
            using (var s = CreateOne())
            {
            }
        }
    }
}

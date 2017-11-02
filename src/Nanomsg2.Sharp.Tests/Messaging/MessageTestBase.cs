using System;
using System.Linq;

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    public abstract class MessageTestBase
    {
        // ReSharper disable once InconsistentNaming
        protected const ulong sizeof_uint = sizeof(uint);

        // ReSharper disable once InconsistentNaming
        protected const string this_is_a_test = "this is a test";

        // ReSharper disable once InconsistentNaming
        protected const string this_is_your_life = "this is your life";

        // ReSharper disable once InconsistentNaming
        protected static readonly byte[] sample_data;

        static MessageTestBase()
        {
            sample_data = Enumerable.Range(0, 16).Select(x => (byte) x).ToArray();
        }

        protected static void VerifyMessage(Action<IMessage> action)
            => VerifyMessage(default(ulong), action);

        protected static void VerifyMessage(ulong sz, Action<IMessage> action)
        {
            Assert.NotNull(action);
            using (var m = new Message(sz))
            {
                action(m);
            }
        }

        protected static void VerifyHavingOne(IHaveOne obj, bool expected)
        {
            Assert.NotNull(obj);
            Assert.Equal(expected, obj.HasOne);
        }

        protected static void VerifySize(IHaveSize obj, ulong expected)
        {
            Assert.NotNull(obj);
            Assert.Equal(expected, obj.Size);
        }
    }
}

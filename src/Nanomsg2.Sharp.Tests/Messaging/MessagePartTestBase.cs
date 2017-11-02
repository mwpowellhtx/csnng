using System;
using System.Collections.Generic;
using System.Linq;

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    public abstract class MessagePartTestBase : MessageTestBase
    {
        private static void VerifyGetCorect(ICanGet<byte[]> part, IEnumerable<byte> expected)
        {
            Assert.NotNull(part);
            /* At this level we cannot know whether what fields should be reversed. That is up to
             * the caller to sort out the context whether what field being reversed makes sense. */
            var actual = part.Get();
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        private static void VerifyGetCorect(IMessage m, IEnumerable<byte> expected
            , Func<IMessage, ICanGet<byte[]>> getPart)
        {
            Assert.NotNull(m);
            Assert.NotNull(getPart);
            // Ditto over VerifyGetVersion.
            VerifyGetCorect(getPart(m), expected);
        }

        private static void VerifyCanAppend<TPart>(Func<IMessage, TPart> getPart
            , uint value, Action<ICanAppend<uint>, uint> verify)
            where TPart : ICanAppend<uint>
        {
            Assert.NotNull(getPart);
            Assert.NotNull(verify);

            VerifyMessage(m =>
            {
                Assert.NotNull(m);
                var part = getPart(m);
                part.Append(value);
                verify(part, value);
            });
        }

        protected static void VerifyCanAppendUInt32<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanAppend<uint>, ICanGet<byte[]>, IHaveSize
        {
            VerifyCanAppend(getPart, value, (p, x) =>
            {
                VerifySize(p as IHaveSize, sizeof_uint);
                /* TODO: TBD: here we must reverse the order of the Bytes. I think that he's reverseing the byte
                 * order putting the value into the buffer. https://github.com/nanomsg/nng/issues/141 */
                VerifyGetCorect(p as ICanGet<byte[]>, BitConverter.GetBytes(value).Reverse());
            });
        }

        private static void VerifyCanPrepend<TPart>(Func<IMessage, TPart> getPart
            , uint value, Action<ICanPrepend<uint>, uint> verify)
            where TPart : ICanPrepend<uint>
        {
            Assert.NotNull(getPart);
            Assert.NotNull(verify);

            VerifyMessage(m =>
            {
                Assert.NotNull(m);
                var part = getPart(m);
                part.Prepend(value);
                verify(part, value);
            });
        }

        protected static void VerifyCanPrependUInt32<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanPrepend<uint>, ICanGet<byte[]>, IHaveSize
        {
            VerifyCanPrepend(getPart, value, (p, x) =>
            {
                VerifySize(p as IHaveSize, sizeof_uint);
                /* TODO: TBD: here we must reverse the order of the Bytes. I think that he's reverseing the byte
                 * order putting the value into the buffer. https://github.com/nanomsg/nng/issues/141 */
                VerifyGetCorect(p as ICanGet<byte[]>, BitConverter.GetBytes(value).Reverse());
            });
        }

        protected static void VerifyTrimLeftSmokeTest<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanAppend<uint>, ICanTrimLeft<uint>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                uint actual;
                var part = getPart(m);
                part.Append(value);
                // Make sure we are actually Trimming the Left most value.
                part.Append(0u);
                Assert.Equal(sizeof_uint * 2, part.Size);
                part.TrimLeft(out actual);
                Assert.Equal(value, actual);
                Assert.Equal(sizeof_uint, part.Size);
            });
        }

        protected static void VerifyTrimRightSmokeTest<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanAppend<uint>, ICanTrimRight<uint>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                uint actual;
                var part = getPart(m);
                part.Append(0u);
                // Ditto Trimming Left, make sure we are actually Trimming Right.
                part.Append(value);
                Assert.Equal(sizeof_uint * 2, part.Size);
                part.TrimRight(out actual);
                Assert.Equal(value, actual);
                Assert.Equal(sizeof_uint, part.Size);
            });
        }

        public abstract void ThatCanAppendUInt32(uint value);

        public abstract void ThatCanPrependUInt32(uint value);

        public abstract void ThatCanTrimLeftUInt32(uint value);

        public abstract void ThatCanTrimRightUInt32(uint value);

        public abstract void VerifyThatAppendBytesNotImplemented(ulong sz);


        public abstract void VerifyThatAppendBytesNoSizeNotImplemented();

        public abstract void VerifyThatAppendStringNotImplemented();

        public abstract void VerifyThatPrependBytesNotImplemented(ulong sz);

        public abstract void VerifyThatPrependBytesNoSizeNotImplemented();

        public abstract void VerifyThatPrependStringNotImplemented();

        public abstract void VerifyThatTrimLeftNotImplemented(ulong sz);

        public abstract void VerifyThatTrimRightNotImplemented(ulong sz);
    }
}

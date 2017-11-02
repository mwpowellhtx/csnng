using System;
using System.Collections.Generic;
using System.Linq;

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    public abstract class MessagePartTestBase : MessageTestBase
    {
        private static void VerifyGetCorect<TPart>(TPart part, IEnumerable<byte> expected)
            where TPart : class, ICanGet<IEnumerable<byte>>
        {
            Assert.NotNull(part);
            /* At this level we cannot know whether what fields should be reversed. That is up to
             * the caller to sort out the context whether what field being reversed makes sense. */
            var actual = part.Get();
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        private static void VerifyGetCorect<TPart>(IMessage m, IEnumerable<byte> expected
            , Func<IMessage, TPart> getPart)
            where TPart : class, ICanGet<IEnumerable<byte>>
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
            where TPart : ICanAppend<uint>, ICanGet<IEnumerable<byte>>, IHaveSize
        {
            VerifyCanAppend(getPart, value, (p, x) =>
            {
                VerifySize(p as IHaveSize, sizeof_uint);
                /* TODO: TBD: here we must reverse the order of the Bytes. I think that he's reverseing the byte
                 * order putting the value into the buffer. https://github.com/nanomsg/nng/issues/141 */
                VerifyGetCorect(p as ICanGet<IEnumerable<byte>>, BitConverter.GetBytes(value).Reverse());
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
            where TPart : ICanPrepend<uint>, ICanGet<IEnumerable<byte>>, IHaveSize
        {
            VerifyCanPrepend(getPart, value, (p, x) =>
            {
                VerifySize(p as IHaveSize, sizeof_uint);
                /* TODO: TBD: here we must reverse the order of the Bytes. I think that he's reverseing the byte
                 * order putting the value into the buffer. https://github.com/nanomsg/nng/issues/141 */
                VerifyGetCorect(p as ICanGet<IEnumerable<byte>>, BitConverter.GetBytes(value).Reverse());
            });
        }

        protected static void VerifyCanAppendBytes<TPart>(ulong sz, IEnumerable<byte> bytes,
            Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<IEnumerable<byte>>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Append(bytes);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                VerifySize(part, sz);
            });
        }

        protected static void VerifyCanPrependBytes<TPart>(ulong sz, IEnumerable<byte> bytes,
            Func<IMessage, TPart> getPart)
            where TPart : class, ICanPrepend<IEnumerable<byte>>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Prepend(bytes);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                VerifySize(part, sz);
            });
        }

        protected static void VerifyCanAppendBytes<TPart>(IEnumerable<byte> bytes,
            Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<IEnumerable<byte>>, IHaveSize
        {
            Assert.NotNull(bytes);
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                // ReSharper disable once PossibleMultipleEnumeration
                part.Append(bytes);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                // ReSharper disable once PossibleMultipleEnumeration
                VerifySize(part, (ulong) bytes.LongCount());
            });
        }

        protected static void VerifyCanPrependBytes<TPart>(IEnumerable<byte> bytes,
            Func<IMessage, TPart> getPart)
            where TPart : class, ICanPrepend<IEnumerable<byte>>, IHaveSize
        {
            Assert.NotNull(bytes);
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                // ReSharper disable once PossibleMultipleEnumeration
                part.Prepend(bytes);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                // ReSharper disable once PossibleMultipleEnumeration
                VerifySize(part, (ulong) bytes.LongCount());
            });
        }

        protected static void VerifyCanAppendString<TPart>(string s, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<string>, IHaveSize
        {
            Assert.NotNull(s);
            Assert.NotEqual(0, s.Length);
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Append(s);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                VerifySize(part, (ulong) s.Length);
            });
        }

        protected static void VerifyCanPrependString<TPart>(string s, Func<IMessage, TPart> getPart)
            where TPart : class, ICanPrepend<string>, IHaveSize
        {
            Assert.NotNull(s);
            Assert.NotEqual(0, s.Length);
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Prepend(s);
                // TODO: TBD: this is sort of the naive view of it. what if we wanted more or less than what was provided in bytes?
                VerifySize(part, (ulong) s.Length);
            });
        }

        protected static void VerifyTrimLeftSmokeTest<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanAppend<uint>, ICanTrimLeft<int, IEnumerable<uint>>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                IEnumerable<uint> actual;
                var part = getPart(m);
                part.Append(value);
                // Make sure we are actually Trimming the Left most value.
                part.Append(0u);
                Assert.Equal(sizeof_uint * 2, part.Size);
                part.TrimLeft(1, out actual);
                Assert.Equal(new[] {value}, actual);
                Assert.Equal(sizeof_uint, part.Size);
            });
        }

        protected static void VerifyTrimRightSmokeTest<TPart>(Func<IMessage, TPart> getPart, uint value)
            where TPart : ICanAppend<uint>, ICanTrimRight<int, IEnumerable<uint>>, IHaveSize
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                IEnumerable<uint> actual;
                var part = getPart(m);
                part.Append(0u);
                // Ditto Trimming Left, make sure we are actually Trimming Right.
                part.Append(value);
                Assert.Equal(sizeof_uint * 2, part.Size);
                part.TrimRight(1, out actual);
                Assert.Equal(new[] {value}, actual);
                Assert.Equal(sizeof_uint, part.Size);
            });
        }

        protected static void VerifyTrimLeftUInt32<TPart>(uint expected, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<uint>, ICanTrimLeft<uint>
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Append(expected);
                uint actual;
                part.TrimLeft(out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimRightUInt32<TPart>(uint expected, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<uint>, ICanTrimRight<uint>
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                part.Append(expected);
                uint actual;
                part.TrimRight(out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimLeftUInt32<TPart>(int count, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<uint>, ICanTrimLeft<int, IEnumerable<uint>>
        {
            Assert.NotNull(getPart);
            VerifyMessage(m =>
            {
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                var expected = Enumerable.Range(0, count).Select(x => (uint) x).ToArray();
                foreach (var x in expected) part.Append(x);
                IEnumerable<uint> actual;
                part.TrimLeft(count, out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimRightUInt32<TPart>(int count, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<uint>, ICanTrimRight<int, IEnumerable<uint>>
        {
            VerifyMessage(m =>
            {
                Assert.NotNull(getPart);
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                var expected = Enumerable.Range(0, count).Select(x => (uint) x).ToArray();
                foreach (var x in expected) part.Append(x);
                IEnumerable<uint> actual;
                part.TrimRight(count, out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimLeftBytes<TPart>(ulong sz, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<IEnumerable<byte>>, ICanTrimLeft<ulong, IEnumerable<byte>>
        {
            VerifyMessage(m =>
            {
                Assert.NotNull(getPart);
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                var expected = Enumerable.Range(0, (int) sz).Select(x => (byte) (x % byte.MaxValue)).ToArray();
                part.Append(expected);
                IEnumerable<byte> actual;
                part.TrimLeft(sz, out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimRightBytes<TPart>(ulong sz, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<IEnumerable<byte>>, ICanTrimRight<ulong, IEnumerable<byte>>
        {
            VerifyMessage(m =>
            {
                Assert.NotNull(getPart);
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                var expected = Enumerable.Range(0, (int)sz).Select(x => (byte)(x % byte.MaxValue)).ToArray();
                part.Append(expected);
                IEnumerable<byte> actual;
                part.TrimRight(sz, out actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimLeftString<TPart>(string expected, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<string>, ICanTrimLeft<int, string>
        {
            Assert.NotNull(expected);
            Assert.NotEqual(0, expected.Length);
            VerifyMessage(m =>
            {
                Assert.NotNull(getPart);
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                part.Append(expected);
                string actual;
                part.TrimLeft(expected.Length, out actual);
                Assert.NotSame(expected, actual);
                Assert.Equal(expected, actual);
            });
        }

        protected static void VerifyTrimRightString<TPart>(string expected, Func<IMessage, TPart> getPart)
            where TPart : class, ICanAppend<string>, ICanTrimRight<int, string>
        {
            Assert.NotNull(expected);
            Assert.NotEqual(0, expected.Length);
            VerifyMessage(m =>
            {
                Assert.NotNull(getPart);
                var part = getPart(m);
                Assert.NotNull(part);
                // TODO: TBD: this is informing the presently naive implementation
                part.Append(expected);
                string actual;
                part.TrimRight(expected.Length, out actual);
                Assert.NotSame(expected, actual);
                Assert.Equal(expected, actual);
            });
        }

        public abstract void ThatCanAppendUInt32(uint value);

        public abstract void ThatCanPrependUInt32(uint value);

        public abstract void ThatCanTrimLeftUInt32(uint value);

        public abstract void ThatCanTrimRightUInt32(uint value);

        public abstract void ThatAppendBytesImpl(ulong sz);


        public abstract void ThatAppendBytesNoSizeImpl(ulong sz);

        public abstract void ThatPrependBytesImpl(ulong sz);

        public abstract void ThatPrependBytesNoSizeImpl(ulong sz);

        public abstract void ThatAppendStringImpl(string s);

        public abstract void ThatPrependStringImpl(string s);

        public abstract void ThatTrimLeftOneUInt32Impl(uint value);

        public abstract void ThatTrimRightOneUInt32Impl(uint value);

        public abstract void ThatTrimLeftUInt32Impl(int count);

        public abstract void ThatTrimRightUInt32Impl(int count);
        
        //public abstract void VerifyThatTrimLeftStringImpl(ulong sz);

        //public abstract void VerifyThatTrimRightStringImpl(ulong sz);

        public abstract void ThatTrimLeftBytesImpl(ulong sz);

        public abstract void ThatTrimRightBytesImpl(ulong sz);

        public abstract void ThatTrimLeftStringImpl(string s);

        public abstract void ThatTrimRightStringImpl(string s);
    }
}

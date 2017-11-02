using System;

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    public class HeaderPartTests : MessagePartTestBase
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfeedface)]
        public override void ThatCanAppendUInt32(uint value)
        {
            VerifyCanAppendUInt32(m => m.Header, value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfeedface)]
        public override void ThatCanPrependUInt32(uint value)
        {
            VerifyCanPrependUInt32(m => m.Header, value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfeedface)]
        public override void ThatCanTrimLeftUInt32(uint value)
        {
            VerifyTrimLeftSmokeTest(m => m.Header, value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfeedface)]
        public override void ThatCanTrimRightUInt32(uint value)
        {
            VerifyTrimRightSmokeTest(m => m.Header, value);
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void VerifyThatAppendBytesNotImplemented(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(new byte[sz], sz));
            });
        }


        [Fact]
        public override void VerifyThatAppendBytesNoSizeNotImplemented()
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(new byte[] {}));
            });
        }

        [Fact]
        public override void VerifyThatAppendStringNotImplemented()
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(string.Empty));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void VerifyThatPrependBytesNotImplemented(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(new byte[sz], sz));
            });
        }

        [Fact]
        public override void VerifyThatPrependBytesNoSizeNotImplemented()
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(new byte[] {}));
            });
        }

        [Fact]
        public override void VerifyThatPrependStringNotImplemented()
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(string.Empty));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void VerifyThatTrimLeftNotImplemented(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.TrimLeft(sz));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void VerifyThatTrimRightNotImplemented(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.TrimRight(sz));
            });
        }
    }
}

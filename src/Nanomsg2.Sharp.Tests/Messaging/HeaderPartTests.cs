//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

using System;
using System.Collections.Generic;

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
        public override void ThatAppendBytesImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(new byte[sz], sz));
            });
        }


        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void ThatAppendBytesNoSizeImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(new byte[sz]));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void ThatPrependBytesImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(new byte[sz], sz));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void ThatPrependBytesNoSizeImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(new byte[sz]));
            });
        }

        [Theory]
        [InlineData(this_is_a_test)]
        [InlineData(this_is_your_life)]
        public override void ThatAppendStringImpl(string s)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Append(s));
            });
        }

        [Theory]
        [InlineData(this_is_a_test)]
        [InlineData(this_is_your_life)]
        public override void ThatPrependStringImpl(string s)
        {
            VerifyMessage(m =>
            {
                Assert.Throws<NotImplementedException>(() => m.Header.Prepend(s));
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfaded)]
        public override void ThatTrimLeftOneUInt32Impl(uint value)
        {
            VerifyTrimLeftUInt32(value, m => m.Header);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0xfaded)]
        public override void ThatTrimRightOneUInt32Impl(uint value)
        {
            VerifyTrimRightUInt32(value, m => m.Header);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public override void ThatTrimLeftUInt32Impl(int count)
        {
            VerifyTrimLeftUInt32(count, m => m.Header);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public override void ThatTrimRightUInt32Impl(int count)
        {
            VerifyTrimRightUInt32(count, m => m.Header);
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void ThatTrimLeftBytesImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                IEnumerable<byte> bytes;
                Assert.Throws<NotImplementedException>(() => m.Header.TrimLeft(sz, out bytes));
            });
        }

        [Theory]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public override void ThatTrimRightBytesImpl(ulong sz)
        {
            VerifyMessage(m =>
            {
                IEnumerable<byte> bytes;
                Assert.Throws<NotImplementedException>(() => m.Header.TrimRight(sz, out bytes));
            });
        }

        [Theory]
        [InlineData(this_is_a_test)]
        [InlineData(this_is_your_life)]
        public override void ThatTrimLeftStringImpl(string s)
        {
            VerifyMessage(m =>
            {
                string actual;
                Assert.Throws<NotImplementedException>(() => m.Header.TrimLeft(s.Length, out actual));
            });
        }

        [Theory]
        [InlineData(this_is_a_test)]
        [InlineData(this_is_your_life)]
        public override void ThatTrimRightStringImpl(string s)
        {
            VerifyMessage(m =>
            {
                string actual;
                Assert.Throws<NotImplementedException>(() => m.Header.TrimRight(s.Length, out actual));
            });
        }
    }
}

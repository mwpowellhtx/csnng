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

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;
    using Xunit.Abstractions;

    // TODO: TBD: establish a Message-only set of unit tests
    // TODO: TBD: establish messagepart-base class tests, and header- and body- specific unit tests.
    // TODO: TBD: should be able to factor that much better...
    public class MessageTests : MessageTestBase
    {
        public MessageTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public void CanCreateMessage(ulong sz)
        {
            VerifyMessage(sz, m =>
            {
                VerifyHavingOne(m, true);
                VerifyHavingOne(m.Header, m.HasOne);
                VerifyHavingOne(m.Body, m.HasOne);

                VerifySize(m.Header, 0ul);
                VerifySize(m.Body, sz);
                VerifySize(m, sz);
            });
        }
    }
}

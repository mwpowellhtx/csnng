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

namespace Nanomsg2.Sharp.Transports
{
    using Xunit;
    using Xunit.Abstractions;
    using static SocketAddressFamily;

    public class SocketAddressFamilyTests : TransportTestBase
    {
        public SocketAddressFamilyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Theory]
        [InlineData(Unspecified, 0)]
        [InlineData(InProcess, 1)]
        [InlineData(InterProcess, 2)]
        [InlineData(IPv4, 3)]
        [InlineData(IPv6, 4)]
        [InlineData(ZeroTier, 5)]
        public void ThatEnumValuesAreCorrect(SocketAddressFamily family, ushort expected)
        {
            Assert.Equal(expected, (ushort) family);
        }
    }
}

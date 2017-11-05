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

namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Xunit.Abstractions;

    public class IPv4PipelineTests : PipelineTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.IPv4;

        public IPv4PipelineTests(ITestOutputHelper @out)
            : base(@out)
        {
        }
    }
}

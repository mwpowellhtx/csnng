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

using Xunit;

namespace Nanomsg2.Sharp.Protocols.Bus
{
    using Xunit.Abstractions;

    public class InterProcessBusTests : BusTests
    {
        protected override SocketAddressFamily Family { get; } = SocketAddressFamily.InterProcess;

        public InterProcessBusTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_default_socket_correct()
        {
            base.That_default_socket_correct();
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out()
        {
            base.That_Bus2_delivers_message_to_Bus1_and_Bus3_times_out();
        }

        [Fact(Skip = "Internal NNG error")]
        public override void That_Bus1_delivers_message_to_both_Bus2_and_Bus3()
        {
            base.That_Bus1_delivers_message_to_both_Bus2_and_Bus3();
        }
    }
}

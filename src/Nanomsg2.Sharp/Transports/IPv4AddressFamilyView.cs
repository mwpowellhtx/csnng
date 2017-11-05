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

namespace Nanomsg2.Sharp
{
    public class IPv4AddressFamilyView : AddressFamilyView<SOCKADDR>, IIPv4AddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.IPv4;

        public uint Address { get; set; }

        public ushort Port { get; set; }

        internal IPv4AddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            Address = @base.IPv4.Address;
            Port = @base.IPv4.Port;
        }
    }
}

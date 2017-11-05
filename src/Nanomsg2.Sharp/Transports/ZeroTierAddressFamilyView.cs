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
    public class ZeroTierAddressFamilyView : AddressFamilyView<SOCKADDR>, IZeroTierAddressFamilyView
    {

        public override ushort Family => (ushort) SocketAddressFamily.ZeroTier;

        public ulong NetworkId { get; set; }

        public ulong NodeId { get; set; }

        public ushort Port { get; set; }

        internal ZeroTierAddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            NetworkId = @base.ZeroTier.NetworkId;
            NodeId = @base.ZeroTier.NodeId;
            Port = @base.ZeroTier.Port;
        }
    }
}

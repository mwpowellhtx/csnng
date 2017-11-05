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
    // ReSharper disable InconsistentNaming
    public enum SocketAddressFamily : ushort
    {
        Unspecified = 0, // ::NNG_AF_UNSPEC,
        InProcess, // ::NNG_AF_INPROC,
        InterProcess, // ::NNG_AF_IPC,
        IPv4, // ::NNG_AF_INET,
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        IPv6, // ::NNG_AF_INET6,
        ZeroTier, // ::NNG_AF_ZT (ZeroTier)
    };
}

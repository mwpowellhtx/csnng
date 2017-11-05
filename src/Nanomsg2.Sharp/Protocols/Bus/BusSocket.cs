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

using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Bus
{
    namespace V0
    {
        using static Imports;
        using static UnmanagedType;

        public class BusSocket : Socket, IBusSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_bus0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public BusSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestBusSocket : V0.BusSocket
    {
    }
}

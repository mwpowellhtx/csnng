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

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Pipeline
{
    using Messaging;
    using static Imports;
    using static UnmanagedType;
    using static SocketFlag;

    namespace V0
    {
        public class PushSocket : Socket, IPushSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_push0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PushSocket()
                : base(__Open)
            {
            }

            public override Message ReceiveMessage(SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(ReceiveMessage));
            }

            public override IEnumerable<byte> ReceiveBytes(ref int count, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(ReceiveBytes));
            }

            public override bool TryReceive(Message message, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(TryReceive));
            }

            public override bool TryReceive(ICollection<byte> buffer, ref int count, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(TryReceive));
            }
        }
    }

    public class LatestPushSocket : V0.PushSocket
    {
    }
}

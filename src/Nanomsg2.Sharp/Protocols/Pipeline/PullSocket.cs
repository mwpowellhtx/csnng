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
        public class PullSocket : Socket, IPullSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_pull0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PullSocket()
                : base(__Open)
            {
            }

            public override void Send(Message message, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }

            public override void Send(IEnumerable<byte> bytes, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }

            public override void Send(IEnumerable<byte> bytes, int count, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }

            public override void Send(IEnumerable<byte> bytes, long count, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }

            public override void Send(string s, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }

            public override void Send(string s, int length, SocketFlag flags = None)
            {
                throw InvalidOperation(nameof(Send));
            }
        }
    }

    public class LatestPullSocket : V0.PullSocket
    {
    }
}

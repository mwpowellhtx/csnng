using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Pair
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class PairSocket : Socket, IPairSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_pair0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PairSocket()
                : base(__Open)
            {
            }
        }
    }

    namespace V1
    {
        public class PairSocket : Socket, IPairSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_pair1_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PairSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestPairSocket : V1.PairSocket
    {
    }
}

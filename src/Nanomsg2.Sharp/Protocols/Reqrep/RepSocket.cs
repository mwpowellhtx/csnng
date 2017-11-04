using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class RepSocket : Socket, IRepSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_rep0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public RepSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestRepSocket : V0.RepSocket
    {
    }
}

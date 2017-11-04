using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Reqrep
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class ReqSocket : Socket, IReqSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_req0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public ReqSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestReqSocket : V0.ReqSocket
    {
    }
}

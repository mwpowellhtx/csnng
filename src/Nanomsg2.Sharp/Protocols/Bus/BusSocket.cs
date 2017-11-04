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

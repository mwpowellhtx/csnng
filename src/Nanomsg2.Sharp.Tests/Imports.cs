using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    internal static class Imports
    {
        public const string NngDll = "nng.dll";

        public const string WinSock2Dll = "WinSock2.dll";

        public const string Ws232Dll = "Ws2_32.dll";

        public const CallingConvention Cdecl = CallingConvention.Cdecl;

        public const CallingConvention StdCall = CallingConvention.StdCall;
    }
}

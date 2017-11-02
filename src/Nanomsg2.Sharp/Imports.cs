using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    internal static class Imports
    {
        /// <summary>
        /// "nng.dll"
        /// </summary>
        public const string NanomsgDll = "nng.dll";

        public const CallingConvention Cdecl = CallingConvention.Cdecl;
    }
}

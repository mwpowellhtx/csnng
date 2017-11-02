using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using static Imports;
    using static Marshal;

    public class NanoException : Exception
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_strerror", CallingConvention = Cdecl)]
        private static extern IntPtr __strerror(int errnum);

        private static string GetStringError(int errnum)
        {
            var sPtr = __strerror(errnum);
            return PtrToStringAnsi(sPtr);
        }

        public int ErrorNumber { get; }

        internal NanoException(int errnum)
            : base(GetStringError(errnum))
        {
            ErrorNumber = errnum;
        }
    }
}

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

using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using static Imports;
    using static Marshal;
    using static ErrorCode;

    public class NanoException : Exception
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_strerror", CallingConvention = Cdecl)]
        private static extern IntPtr __strerror(int errnum);

        private static string GetStringError(int errnum)
        {
            var sPtr = __strerror(errnum);
            return PtrToStringAnsi(sPtr);
        }

        public bool WasSystemError { get; }

        public bool WasTransportError { get; }

        public int RawNumber { get; }

        public int ErrorNumber { get; }

        internal NanoException(int errnum)
            : base(GetStringError(errnum & ~((int) SystemError | (int) TransportError)))
        {
            WasSystemError = (errnum & (int) SystemError) == (int) SystemError;
            WasTransportError = (errnum & (int) TransportError) == (int) TransportError;
            RawNumber = errnum;
            ErrorNumber = errnum & ~((int) SystemError | (int) TransportError);
        }
    }
}

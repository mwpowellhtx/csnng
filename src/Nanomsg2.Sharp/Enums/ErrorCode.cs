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

namespace Nanomsg2.Sharp
{
    [Flags]
    public enum ErrorCode : int
    {
        None = 0,
        /* We are not ready for SWIG exception comprehension yet, which may take some doing. However, let's
        get set for it eventually anyway. Order matters, but we can leverage the automatic incrementing. */
        Interrupted, // = ::NNG_EINTR,
        NoMemory, // = ::NNG_ENOMEM,
        Invalid, // = NNG_EINVAL,
        Busy, // = NNG_EBUSY,
        TimedOut, // = NNG_ETIMEDOUT,
        ConnectionRefused, // = NNG_ECONNREFUSED,
        Closed, // = NNG_ECLOSED,
        Again, // = NNG_EAGAIN,
        NotSupported, // = NNG_ENOTSUP,
        AddressInUse, // = NNG_EADDRINUSE,
        State, // = NNG_ESTATE,
        NoEntry, // = NNG_ENOENT,
        Proto, // = NNG_EPROTO,
        Unreachable, // = NNG_EUNREACHABLE,
        AddressInvalid, // = NNG_EADDRINVAL,
        Perm, // = NNG_EPERM,
        MessageSize, // = NNG_EMSGSIZE,
        ConnectionAborted, // = NNG_ECONNABORTED,
        ConnectionReset, // = NNG_ECONNRESET,
        Canceled, // = NNG_ECANCELED,
        NoFiles, // = NNG_ENOFILES,
        NoSpace, // = NNG_ENOSPC,
        Exists, // = NNG_EEXIST,
        ReadOnly, // = NNG_EREADONLY,
        WriteOnly, // = NNG_EWRITEONLY,
        // Then these values need to be defined specifically, and agreement with the core C API.
        Internal = 1000, // NNG_EINTERNAL,
        /* These are less of an enumeration and more of a mask... potentially.
        Especially these two, SYSERR and TRANERR. */
        SystemError = 0x10000000, // NNG_ESYSERR,
        TransportError = 0x20000000, // NNG_ETRANERR,
    };

    public static class ErrorCodeExtensionMethods
    {
        public static ErrorCode ToErrorCode(this int value) => (ErrorCode) value;
    }
}

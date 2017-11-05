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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    using static Imports;
    using static UnmanagedType;

    public class HeaderPart : MessagePart, IHeaderPart
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_len", CallingConvention = Cdecl)]
        [return: MarshalAs(I8)]
        private static extern ulong __GetLength(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header", CallingConvention = Cdecl)]
        private static extern IntPtr __GetBytes(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_clear", CallingConvention = Cdecl)]
        private static extern void __Clear(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_append_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __AppendUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_insert_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __PrependUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_trim_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimLeftUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_chop_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimRightUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        internal HeaderPart(IMessage message)
            : base(message)
        {
        }

        public override ulong Size => Invoker.InvokeWithResult(__GetLength);

        public override void Clear()
        {
            Invoker.InvokeHavingNoResult(__Clear);
        }

        public override IEnumerable<byte> Get()
        {
            var data = Invoker.InvokeWithResult(__GetBytes);
            return DecodeGetResult(data);
        }

        public override void Append(uint value)
        {
            Invoker.InvokeWithDefaultErrorHandling(ptr => __AppendUInt32(ptr, value));
        }

        public override void Prepend(uint value)
        {
            Invoker.InvokeWithDefaultErrorHandling(ptr => __PrependUInt32(ptr, value));
        }

        protected override uint TrimLeft()
        {
            var x = default(uint);
            Invoker.InvokeWithDefaultErrorHandling(ptr => __TrimLeftUInt32(ptr, ref x));
            return x;
        }

        protected override uint TrimRight()
        {
            var x = default(uint);
            Invoker.InvokeWithDefaultErrorHandling(ptr => __TrimRightUInt32(ptr, ref x));
            return x;
        }
    }
}

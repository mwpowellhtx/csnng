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
using System.Linq;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    using static Math;
    using static Convert;
    using static Imports;
    using static UnmanagedType;

    public class BodyPart : MessagePart, IBodyPart
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_msg_len", CallingConvention = Cdecl)]
        [return: MarshalAs(U8)]
        private static extern ulong __GetLength(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_body", CallingConvention = Cdecl)]
        private static extern IntPtr __GetBytes(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_clear", CallingConvention = Cdecl)]
        private static extern void __Clear(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __AppendUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_insert_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __PrependUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_trim_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimLeftUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_chop_u32", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimRightUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __AppendByteBuffer(IntPtr msgPtr, [MarshalAs(LPArray)] byte[] buffer, [MarshalAs(U8)] ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __PrependByteBuffer(IntPtr msgPtr, [MarshalAs(LPArray)] byte[] buffer, [MarshalAs(U8)]  ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_trim", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimBytesLeft(IntPtr msgPtr, [MarshalAs(U8)] ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_chop", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __TrimBytesRight(IntPtr msgPtr, [MarshalAs(U8)] ulong sz);

        internal BodyPart(IMessage message)
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

        public override void Append(IEnumerable<byte> buffer, ulong sz)
        {
            var b = (buffer ?? new byte[0]).ToArray();
            Invoker.InvokeWithDefaultErrorHandling(ptr => __AppendByteBuffer(
                ptr, b, Min(sz, (ulong) b.Length))
            );
        }

        public override void Append(IEnumerable<byte> buffer)
        {
            var b = buffer ?? new byte[0];
            Append(b, (ulong) b.LongCount());
        }

        public override void Append(string s)
        {
            var buffer = (s ?? string.Empty).Select(x => (byte) x).ToArray();
            Append(buffer);
        }

        public override void Prepend(IEnumerable<byte> buffer, ulong sz)
        {
            var b = (buffer ?? new byte[0]).ToArray();
            Invoker.InvokeWithDefaultErrorHandling(ptr => __PrependByteBuffer(
                ptr, b, Min(sz, (ulong) b.Length))
            );
        }

        public override void Prepend(IEnumerable<byte> buffer)
        {
            var b = buffer ?? new byte[0];
            Prepend(b, (ulong) b.LongCount());
        }

        public override void Prepend(string s)
        {
            var buffer = (s ?? string.Empty).Select(x => (byte) x).ToArray();
            Prepend(buffer);
        }

        public override void TrimLeft(ulong sz, out IEnumerable<byte> result)
        {
            // TODO: TBD: we will start by assuming there is sufficient size in the buffer.
            // TODO: TBD: I realize that is a naive look but will specialize the use cases a bit later
            var local = Get();
            result = local.Take((int) sz).ToArray();
            Invoker.InvokeWithDefaultErrorHandling(ptr => __TrimBytesLeft(ptr, sz));
        }

        public override void TrimRight(ulong sz, out IEnumerable<byte> result)
        {
            // TODO: TBD: ditto TrimLeft re: size assumptions
            var local = Get().ToArray();
            result = local.Skip(local.Length - (int) sz).ToArray();
            Invoker.InvokeWithDefaultErrorHandling(ptr => __TrimBytesRight(ptr, sz));
        }

        public override void TrimLeft(int length, out string result)
        {
            // TODO: TBD: this is a somewhat naive perspective. there may be better ways of doing this...
            IEnumerable<byte> bytes;
            TrimLeft((ulong) length, out bytes);
            result = new string(bytes.Select(ToChar).ToArray());
        }

        public override void TrimRight(int length, out string result)
        {
            // TODO: TBD: ditto Trimleft...
            IEnumerable<byte> bytes;
            TrimRight((ulong)length, out bytes);
            result = new string(bytes.Select(ToChar).ToArray());
        }
    }
}

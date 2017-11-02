using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    using static Math;
    using static Imports;
    using static UnmanagedType;

    public class BodyPart : MessagePart, IBodyPart
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_msg_len", CallingConvention = Cdecl)]
        private static extern ulong __GetLength(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_body", CallingConvention = Cdecl)]
        private static extern IntPtr __GetBytes(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_clear", CallingConvention = Cdecl)]
        private static extern void __Clear(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append_u32", CallingConvention = Cdecl)]
        private static extern int __AppendUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_insert_u32", CallingConvention = Cdecl)]
        private static extern int __PrependUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_trim_u32", CallingConvention = Cdecl)]
        private static extern int __TrimLeftUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_chop_u32", CallingConvention = Cdecl)]
        private static extern int __TrimRightUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append", CallingConvention = Cdecl)]
        private static extern int __AppendByteBuffer(IntPtr msgPtr, [MarshalAs(LPArray)] byte[] buffer, [MarshalAs(U8)] ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_append", CallingConvention = Cdecl)]
        private static extern int __PrependByteBuffer(IntPtr msgPtr, [MarshalAs(LPArray)] byte[] buffer, [MarshalAs(U8)]  ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_trim", CallingConvention = Cdecl)]
        private static extern int __TrimBytesLeft(IntPtr msgPtr, [MarshalAs(U8)] ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_chop", CallingConvention = Cdecl)]
        private static extern int __TrimBytesRight(IntPtr msgPtr, [MarshalAs(U8)] ulong sz);

        internal BodyPart(Message message)
            : base(message)
        {
        }

        public override ulong Size => ProtectedParent.InvokeWithResult(__GetLength);

        public override void Clear()
        {
            ProtectedParent.InvokeHavingNoResult(__Clear);
        }

        public override byte[] Get()
        {
            var data = ProtectedParent.InvokeWithResult(__GetBytes);
            return DecodeGetResult(data);
        }

        public override void Append(uint value)
        {
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __AppendUInt32(ptr, value));
        }

        public override void Prepend(uint value)
        {
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __PrependUInt32(ptr, value));
        }

        public override void TrimLeft(out uint value)
        {
            var temp = default(uint);
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimLeftUInt32(ptr, ref temp));
            value = temp;
        }

        public override void TrimRight(out uint value)
        {
            var temp = default(uint);
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimRightUInt32(ptr, ref temp));
            value = temp;
        }

        public override void Append(IEnumerable<byte> buffer, ulong sz)
        {
            var b = (buffer ?? new byte[0]).ToArray();
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __AppendByteBuffer(
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
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __PrependByteBuffer(
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

        public override void TrimLeft(ulong sz)
        {
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimBytesLeft(ptr, sz));
        }

        public override void TrimRight(ulong sz)
        {
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimBytesRight(ptr, sz));
        }
    }
}

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
        private static extern ulong __GetLength(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header", CallingConvention = Cdecl)]
        private static extern IntPtr __GetBytes(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_clear", CallingConvention = Cdecl)]
        private static extern void __Clear(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_append_u32", CallingConvention = Cdecl)]
        private static extern int __AppendUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_insert_u32", CallingConvention = Cdecl)]
        private static extern int __PrependUInt32(IntPtr msgPtr, [MarshalAs(U4)] uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_trim_u32", CallingConvention = Cdecl)]
        private static extern int __TrimLeftUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_header_chop_u32", CallingConvention = Cdecl)]
        private static extern int __TrimRightUInt32(IntPtr msgPtr, [MarshalAs(U4)] ref uint value);

        internal HeaderPart(Message message)
            : base(message)
        {
        }

        public override ulong Size => ProtectedParent.InvokeWithResult(__GetLength);

        public override void Clear()
        {
            ProtectedParent.InvokeHavingNoResult(__Clear);
        }

        public override IEnumerable<byte> Get()
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

        protected override uint TrimLeft()
        {
            var x = default(uint);
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimLeftUInt32(ptr, ref x));
            return x;
        }

        protected override uint TrimRight()
        {
            var x = default(uint);
            ProtectedParent.InvokeWithDefaultErrorHandling(ptr => __TrimRightUInt32(ptr, ref x));
            return x;
        }
    }
}

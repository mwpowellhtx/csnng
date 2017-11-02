using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    using static Imports;

    public class Message : Disposable, IMessage
    {
        private IntPtr _msgPtr = IntPtr.Zero;

        // TODO: TBD: probably belongs in another class... along the same lines as "invocation" in C++
        internal delegate void InvocationHavingNoResult<in T>(T ptr);

        internal delegate TResult InvocationWithResultDelegate<in T, out TResult>(T ptr);

        internal void InvokeHavingNoResult<T>(InvocationHavingNoResult<T> caller, T ptr)
        {
            Allocate(ref _msgPtr);
            caller(ptr);
        }

        internal void InvokeHavingNoResult(InvocationHavingNoResult<IntPtr> caller)
        {
            InvokeHavingNoResult(caller, _msgPtr);
        }

        internal void InvokeWithDefaultErrorHandling<T>(InvocationWithResultDelegate<T, int> caller, T ptr)
        {
            Allocate(ref _msgPtr);
            var result = caller(ptr);
            // TODO: TBD: do something with the result...
        }

        internal TResult InvokeWithResult<T, TResult>(InvocationWithResultDelegate<T, TResult> caller, T ptr)
        {
            Allocate(ref _msgPtr);
            var result = caller(ptr);
            return result;
        }

        internal void InvokeWithDefaultErrorHandling(InvocationWithResultDelegate<IntPtr, int> caller)
        {
            Allocate(ref _msgPtr);
            var result = caller(_msgPtr);
            if (result == 0) return;
            // TODO: TBD: do something with the result...
            // TODO: TBD: introduce an appropriately named exception
            var ex = new Exception();
            ex.Data.Add("retval", result);
            throw ex;
        }

        internal TResult InvokeWithResult<TResult>(InvocationWithResultDelegate<IntPtr, TResult> caller)
        {
            return InvokeWithResult(caller, _msgPtr);
        }

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_alloc", CallingConvention = Cdecl)]
        private static extern int __Allocate(out IntPtr msgPtr, ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_free", CallingConvention = Cdecl)]
        private static extern int __Free(IntPtr msgPtr);

        public IHeaderPart Header { get; }

        public IBodyPart Body { get; }

        public Message()
            : this(0)
        {
        }

        public Message(ulong sz)
        {
            Allocate(ref _msgPtr, sz);
            Header = new HeaderPart(this);
            Body = new BodyPart(this);
        }

        ~Message()
        {
            Dispose(false);
        }

        public bool HasOne => _msgPtr != IntPtr.Zero;

        private static ulong GetSize(IHaveSize part) => part?.Size ?? default(ulong);

        public ulong Size => GetSize(Header) + GetSize(Body);

        public void Clear()
        {
            Header.Clear();
            Body.Clear();
        }

        protected void Allocate(ref IntPtr msgPtr, ulong sz = 0)
        {
            if (HasOne) return;
            __Allocate(out _msgPtr, sz);
        }

        public virtual void Free()
        {
            if (!HasOne) return;
            __Free(_msgPtr);
            _msgPtr = IntPtr.Zero;
        }

        // TODO: TBD: separate the Disposal out into a base class...
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && !IsDisposed)
            {
                Free();
            }
        }
    }
}

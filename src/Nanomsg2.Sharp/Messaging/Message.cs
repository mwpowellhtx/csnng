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

namespace Nanomsg2.Sharp.Messaging
{
    using static Imports;

    public class Message : Invoker, IMessage, ISameAs<Message>
    {
        private IntPtr _msgPtr;

        internal IntPtr MsgPtr => _msgPtr;

        internal void InvokeHavingNoResult(InvocationHavingNoResult<IntPtr> caller)
        {
            // We override these because we want to perform special handling of the underlying Message Ptr.
            Allocate(ref _msgPtr);
            InvokeHavingNoResult(caller, _msgPtr);
        }

        internal TResult InvokeWithResult<TResult>(InvocationWithResultDelegate<IntPtr, TResult> caller)
        {
            // Ditto Message ptr alignment.
            Allocate(ref _msgPtr);
            return InvokeWithResult(caller, _msgPtr);
        }

        internal void InvokeWithDefaultErrorHandling(InvocationWithResultDelegate<IntPtr, int> caller)
        {
            // Make sure that the Message we allocate is the same one we pass for Invocation.
            Allocate(ref _msgPtr);
            base.InvokeWithDefaultErrorHandling(caller, _msgPtr);
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

        protected internal Message(IntPtr msgPtr)
            : this(0)
        {
            RetainPtr(msgPtr);
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

        private static bool SameAs(Message a, Message b)
        {
            return !(a == null || b == null)
                   && (ReferenceEquals(a, b) || a._msgPtr == b._msgPtr);
        }

        public virtual bool SameAs(Message other) => SameAs(this, other);

        internal IntPtr CedePtr()
        {
            var msgPtr = _msgPtr;
            _msgPtr = IntPtr.Zero;
            return msgPtr;
        }

        internal void RetainPtr(IntPtr msgPtr)
        {
            Free();
            _msgPtr = msgPtr;
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
            if (disposing && !IsDisposed)
            {
                Free();
            }

            base.Dispose(disposing);
        }
    }
}

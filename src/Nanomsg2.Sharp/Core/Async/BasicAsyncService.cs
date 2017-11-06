using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using static Imports;
    using static UnmanagedType;

    // TODO: TBD: make this one public? or internal for internal use only?
    public delegate void BasicAsyncCallback();

    public class BasicAsyncService : Invoker, IAsyncService
    {
        private delegate void PrivateAsyncCallback(IntPtr argPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_alloc", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Alloc(out IntPtr aioPtr, PrivateAsyncCallback callback, IntPtr argPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_free", CallingConvention = Cdecl)]
        private static extern void __Free(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_wait", CallingConvention = Cdecl)]
        private static extern void __Wait(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_stop", CallingConvention = Cdecl)]
        private static extern void __Stop(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_cancel", CallingConvention = Cdecl)]
        private static extern void __Cancel(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_result", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetResult(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_set_timeout", CallingConvention = Cdecl)]
        private static extern void __SetTimeoutDurationMilliseconds(IntPtr aioPtr, int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_get_msg", CallingConvention = Cdecl)]
        private static extern IntPtr __GetMessagePtr(IntPtr aioPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_aio_set_msg", CallingConvention = Cdecl)]
        private static extern void __SetMessagePtr(IntPtr aioPtr, IntPtr msgPtr);

        private delegate void SetMessagePtrDelegate(IntPtr msgPtr);

        private InvocationHavingNoResult _free;

        private InvocationHavingNoResult _wait;

        private InvocationHavingNoResult _stop;

        private InvocationHavingNoResult _cancel;

        private InvocationWithResultDelegate<int> _getResult;

        private InvocationWithResultDelegate<IntPtr> _getMessagePtr;

        private SetMessagePtrDelegate _setMessagePtr;

        private AsyncOptionWriter PrivateOptions { get; }

        public IAsyncOptionWriter Options { get; }

        public BasicAsyncService()
            : this(() => { })
        {
        }

        public BasicAsyncService(BasicAsyncCallback callback)
        {
            Options = PrivateOptions = new AsyncOptionWriter(this);
            Start(callback);
        }

        private IntPtr _aioPtr;

        internal IntPtr AioPtr => _aioPtr;

        public virtual bool HasOne => _aioPtr != IntPtr.Zero;

        private BasicAsyncCallback _callback;

        private void PrivateCallback(IntPtr argPtr)
        {
            // TODO: TBD: may want to pass the Result along to the caller.
            _callback();
        }

        public void Start(BasicAsyncCallback callback)
        {
            _callback = callback;
            Start();
        }

        public virtual void Start()
        {
            if (HasOne) return;
            InvokeWithDefaultErrorHandling(() => __Alloc(out _aioPtr, PrivateCallback, IntPtr.Zero));
            Configure(_aioPtr);
        }

        private void Configure(IntPtr aioPtr)
        {
            PrivateOptions.SetSetters(
                x => __SetTimeoutDurationMilliseconds(_aioPtr, x)
            );

            // TODO: TBD: perhaps we can run direct through the C calling conventions.
            _free = () => __Free(aioPtr);
            _wait = () => __Wait(aioPtr);
            _stop = () => __Stop(aioPtr);
            _cancel = () => __Cancel(aioPtr);
            _getResult = () => __GetResult(aioPtr);
            _getMessagePtr = () => __GetMessagePtr(aioPtr);
            _setMessagePtr = msgPtr => __SetMessagePtr(aioPtr, msgPtr);
        }

        private void Free()
        {
            if (!HasOne) return;
            InvokeHavingNoResult(_free);
            _aioPtr = IntPtr.Zero;
        }

        public virtual void Wait()
        {
            InvokeHavingNoResult(_wait);
        }

        public virtual void TimedWait(int timeoutMilliseconds)
        {
            Options.SetTimeoutDurationMilliseconds(timeoutMilliseconds);
            Wait();
        }

        public virtual void TimedWait(TimeSpan timeout)
        {
            Options.SetTimeoutDuration(timeout);
            Wait();
        }

        public virtual void Close()
        {
            Close(false);
        }

        public virtual void Close(bool force)
        {
            // TODO: TBD: do we need to Wait after Cancel?
            if (force) Cancel();
            else
            {
                Stop();
                Wait();
            }
            Free();
            // Which the Ptr should be Zeroed at this point.
            Configure(_aioPtr);
        }

        public virtual void Stop()
        {
            InvokeHavingNoResult(() => _stop());
        }

        public virtual void Cancel()
        {
            InvokeHavingNoResult(_cancel);
        }

        public virtual int Result => InvokeWithResult(_getResult);

        public virtual void VerifyResult()
        {
            InvokeWithDefaultErrorHandling(_getResult);
        }

        public virtual bool Success
        {
            get
            {
                VerifyResult();
                return true;
            }
        }

        public virtual bool TrySuccess
        {
            get
            {
                try
                {
                    return Success;
                }
                catch
                {
                    return false;
                }
            }
        }

        public virtual void Retain(Message message)
        {
            InvokeHavingNoResult(() => _setMessagePtr(message.CedePtr()));
        }

        public virtual void Cede(Message message)
        {
            var msgPtr = InvokeWithResult(_getMessagePtr);
            message.RetainPtr(msgPtr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                Close(true);

                PrivateOptions.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

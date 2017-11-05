using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Nanomsg2.Sharp.Messaging
{
    using static Imports;
    using static UnmanagedType;
    using static ErrorCode;

    public class MessagePipe : Invoker, IMessagePipe, ISameAs<MessagePipe>
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_msg_get_pipe", CallingConvention = Cdecl)]
        [return: MarshalAs(U4)]
        private static extern uint __GetPipe(IntPtr msgPtr);

        [DllImport(NanomsgDll, EntryPoint = "nng_msg_set_pipe", CallingConvention = Cdecl)]
        private static extern void __SetPipe(IntPtr msgPtr, uint pid);

        [DllImport(NanomsgDll, EntryPoint = "nng_pipe_close", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Close(uint pid);

        [DllImport(NanomsgDll, EntryPoint = "nng_pipe_getopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptInt32(uint pid, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_pipe_getopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptUInt64(uint pid, [MarshalAs(LPStr)] string name, ref ulong value);

        [DllImport(NanomsgDll, EntryPoint = "nng_pipe_getopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptStringBuilder(uint sid, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder sb, ref ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_pipe_getopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptDurationMilliseconds(uint pid, [MarshalAs(LPStr)] string name, ref int value);

        private uint _pid;

        public virtual bool HasOne => _pid != 0;

        private IntPtr _msgPtr;

        public virtual bool HasMessage => _msgPtr != IntPtr.Zero;

        private InvocationHavingNoResult<IntPtr> _reset;

        private InvocationWithResultDelegate<int> _closer;

        private OptionReader PrivateOptions { get; }

        public IOptionReader Options { get; }

        public MessagePipe(Message message)
        {
            Options = PrivateOptions = new OptionReader();
            Configure(_pid = InvokeWithResult(__GetPipe, _msgPtr = message.MsgPtr));
        }

        private void Configure(uint pid)
        {
            _closer = () => __Close(pid);

            _reset = ptr => __SetPipe(ptr, pid);

            var opt = PrivateOptions;

            opt.SetGetters(
                (string name, ref int value) => __GetOptInt32(pid, name, ref value)
                , (string name, ref ulong value) => __GetOptUInt64(pid, name, ref value)
                , (string name, StringBuilder value, ref ulong length) => __GetOptStringBuilder(
                    pid, name, value, ref length)
                , (string name, ref int value) => __GetOptDurationMilliseconds(pid, name, ref value)
            );
        }

        public void Close()
        {
            if (!HasOne) return;
            InvokeWithDefaultErrorHandling(_closer, None, NoEntry);
            Configure(_pid = default(uint));
        }

        private static bool SameAs(MessagePipe a, MessagePipe b)
        {
            return !(a == null || b == null)
                   && (ReferenceEquals(a, b) || (a._pid == b._pid));
        }

        public bool SameAs(MessagePipe other) => SameAs(this, other);

        private void Set(IntPtr msgPtr)
        {
            Configure(_pid);
            InvokeHavingNoResult(ptr => __SetPipe(ptr, _pid), msgPtr);
        }

        public void Set()
        {
            Set(_msgPtr);
        }

        public void Set(Message message)
        {
            if (HasOne) return;
            Set(_msgPtr = message.MsgPtr);
        }

        public void Reset()
        {
            if (!HasOne) return;
            /* The only thing we should be doing here is reconnecting the Pipe
             * with the Message. Which does not obviate the Pipe, apparently. */
            InvokeHavingNoResult(_reset, _msgPtr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                Close();
            }

            base.Dispose(disposing);
        }
    }
}

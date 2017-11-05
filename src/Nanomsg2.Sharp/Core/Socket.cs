using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using static Imports;
    using static UnmanagedType;
    using static SocketFlag;

    // TODO: TBD: consider: IProtocol "declares" whether Sender, Receiver, or Both...
    public abstract class Socket : Invoker, ISocket
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_close", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Close(uint sid);

        [DllImport(NanomsgDll, EntryPoint = "nng_listen", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Listen(uint sid, [MarshalAs(LPStr)] string addr, ref uint lid, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_dial", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Dial(uint sid, [MarshalAs(LPStr)] string addr, ref uint did, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_getopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptInt32(uint sid, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_getopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptUInt64(uint sid, [MarshalAs(LPStr)] string name, ref ulong value);

        [DllImport(NanomsgDll, EntryPoint = "nng_getopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptStringBuilder(uint sid, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder sb, ref ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_getopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptDurationMilliseconds(uint sid, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_setopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptInt32(uint sid, [MarshalAs(LPStr)] string name, int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_setopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptUInt64(uint sid, [MarshalAs(LPStr)] string name, ulong value);

        [DllImport(NanomsgDll, EntryPoint = "nng_setopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptStringBuilder(uint sid, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder sb, ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_setopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptDurationMilliseconds(uint sid, [MarshalAs(LPStr)] string name, int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_send", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Send(uint sid, [MarshalAs(LPStr)] StringBuilder sb, ulong length, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_send", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Send(uint sid, [MarshalAs(LPArray)] byte[] bytes, ulong length, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_sendmsg", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SendMessage(uint sid, IntPtr msgPtr, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_recv", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Receive(uint sid, [MarshalAs(LPArray)] byte[] buffer, ref ulong sz, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_recvmsg", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __ReceiveMessage(uint sid, out IntPtr msgPtr, int flags);

        // TODO: TBD: there is really no pretty way to handle this from a C# language perspective but to expose the Id for internal use
        private uint _sid;

        internal uint Id => _sid;

        public bool HasOne => Id != 0;

        private OptionReaderWriter PrivateOptions { get; }

        public IOptionReaderWriter Options { get; }

        protected delegate int OpenDelegate(ref uint sid);

        protected Socket(OpenDelegate open)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => open(ref _sid));
            Options = PrivateOptions = new OptionReaderWriter();
            Configure(_sid);
        }

        protected static NotImplementedException NotImplemented(string name)
        {
            return new NotImplementedException($"{name} is not implemented.");
        }

        protected virtual InvalidOperationException InvalidOperation(string name)
        {
            return new InvalidOperationException($"{name} operation invalid for {GetType().FullName}.");
        }

        private void Configure(uint sid)
        {
            var opt = PrivateOptions;

            opt.SetGetters(
                (string name, ref int value) => __GetOptInt32(sid, name, ref value)
                , (string name, ref ulong value) => __GetOptUInt64(sid, name, ref value)
                , (string name, StringBuilder value, ref ulong length) => __GetOptStringBuilder(
                    sid, name, value, ref length)
                , (string name, ref int value) => __GetOptDurationMilliseconds(sid, name, ref value)
            );

            opt.SetSetters(
                (name, value) => __SetOptInt32(sid, name, value)
                , (name, value) => __SetOptUInt64(sid, name, value)
                , (name, value, length) => __SetOptStringBuilder(sid, name, new StringBuilder(value), length)
                , (name, value) => __SetOptDurationMilliseconds(sid, name, value)
            );
        }

        public void Close()
        {
            if (!HasOne) return;
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Close(_sid));
            Configure(_sid = 0);
        }

        public void Listen(string addr, SocketFlag flags = None)
        {
            Listen(addr, null, flags);
        }

        public void Listen(string addr, Listener l, SocketFlag flags = None)
        {
            var lid = default(uint);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Listen(_sid, addr, ref lid, (int) flags));
            l?.OnListened(lid);
        }

        public void Dial(string addr, SocketFlag flags = None)
        {
            Dial(addr, null, flags);
        }

        public void Dial(string addr, Dialer d, SocketFlag flags = None)
        {
            var did = default(uint);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Dial(_sid, addr, ref did, (int) flags));
            d?.OnDialed(did);
        }

        public virtual void Send(Message message, SocketFlag flags = None)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __SendMessage(_sid, message.CedePtr(), (int) flags));
        }

        public virtual void Send(IEnumerable<byte> bytes, SocketFlag flags = None)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Send(bytes, bytes.LongCount(), flags);
            // ReSharper enable PossibleMultipleEnumeration
        }

        public virtual void Send(IEnumerable<byte> bytes, int count, SocketFlag flags = None)
        {
            Send(bytes, (long) count, flags);
        }

        public virtual void Send(IEnumerable<byte> bytes, long count, SocketFlag flags = None)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Send(_sid
                , bytes.ToArray(), (ulong) count, (int) flags));
        }

        public virtual void Send(string s, SocketFlag flags = None)
        {
            Send(s, s.Length, flags);
        }

        public virtual void Send(string s, int length, SocketFlag flags = None)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Send(_sid
                , new StringBuilder(s), (ulong) length, (int) flags));
        }

        public virtual Message ReceiveMessage(SocketFlag flags = None)
        {
            var message = new Message();
            return TryReceive(message, flags) ? message : null;
        }

        public virtual bool TryReceive(Message message, SocketFlag flags = None)
        {
            var msgPtr = IntPtr.Zero;
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __ReceiveMessage(_sid, out msgPtr, (int) flags));
            message.RetainPtr(msgPtr);
            return message.HasOne;
        }

        public virtual IEnumerable<byte> ReceiveBytes(ref int count, SocketFlag flags = None)
        {
            var buffer = new List<byte>();
            return TryReceive(buffer, ref count, flags) ? buffer : null;
        }

        public virtual bool TryReceive(ICollection<byte> buffer, ref int count, SocketFlag flags = None)
        {
            var local = new byte[count];
            var sz = (ulong) count;
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Receive(_sid, local, ref sz, (int)flags));
            buffer.AddRange(local.Take(count = (int) sz));
            return sz > 0;
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

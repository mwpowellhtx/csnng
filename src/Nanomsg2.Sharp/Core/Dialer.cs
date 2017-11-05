using System.Runtime.InteropServices;
using System.Text;

namespace Nanomsg2.Sharp
{
    using static Imports;
    using static UnmanagedType;

    public class Dialer : EndPoint, IDialer
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_create", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Create(ref uint did, uint sid, [MarshalAs(LPStr)] string addr);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_start", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Start(uint did, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_close", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Close(uint did);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_getopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptInt32(uint did, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_getopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptUInt64(uint did, [MarshalAs(LPStr)] string name, ref ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_getopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptStringBuilder(uint did, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder value, ref ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_getopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptDurationMilliseconds(uint did, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_setopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptInt32(uint did, [MarshalAs(LPStr)] string name, int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_setopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptUInt64(uint did, [MarshalAs(LPStr)] string name, ulong sz);


        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_setopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptStringBuilder(uint did, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder value, ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_dialer_setopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptDurationMilliseconds(uint did, [MarshalAs(LPStr)] string name, int value);

        private uint _did;

        protected internal override uint Id => _did;

        public Dialer(Socket s, string addr)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Create(ref _did, s.Id, addr));
            Configure(_did);
        }

        public Dialer()
        {
        }

        internal void OnDialed(uint did)
        {
            Close();
            Configure(_did = did);
        }

        private void Configure(uint did)
        {
            // See: Listener Configuration comments!

            // This is pretty amazing!
            SetDelegates(
                flags => __Start(did, flags)
                , () => __Close(did)
            );

            var opt = ProtectedOptions;

            // Once again, pretty amazing!
            opt.SetGetters(
                (string name, ref int value) => __GetOptInt32(did, name, ref value)
                , (string name, ref ulong value) => __GetOptUInt64(did, name, ref value)
                , (string name, StringBuilder value, ref ulong length) => __GetOptStringBuilder(
                    did, name, value, ref length)
                , (string name, ref int value) => __GetOptDurationMilliseconds(did, name, ref value)
            );

            // Ditto amazing!
            opt.SetSetters(
                (name, value) => __SetOptInt32(did, name, value)
                , (name, sz) => __SetOptUInt64(did, name, sz)
                , (name, value, length) => __SetOptStringBuilder(did, name, new StringBuilder(value), length)
                , (name, value) => __SetOptDurationMilliseconds(did, name, value)
            );
        }

        public override void Close()
        {
            base.Close();
            _did = 0;
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

using System.Runtime.InteropServices;
using System.Text;

namespace Nanomsg2.Sharp
{
    using static Imports;
    using static UnmanagedType;

    public class Listener : EndPoint, IListener
    {
        [DllImport(NanomsgDll, EntryPoint = "nng_listener_create", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Create(ref uint lid, uint sid, [MarshalAs(LPStr)] string addr);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_start", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Start(uint lid, int flags);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_close", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __Close(uint lid);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_getopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptInt32(uint lid, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_getopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptUInt64(uint lid, [MarshalAs(LPStr)] string name, ref ulong sz);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_getopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptStringBuilder(uint lid, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder value, ref ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_getopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __GetOptDurationMilliseconds(uint lid, [MarshalAs(LPStr)] string name, ref int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_setopt_int", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptInt32(uint lid, [MarshalAs(LPStr)] string name, int value);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_setopt_size", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptUInt64(uint lid, [MarshalAs(LPStr)] string name, ulong sz);


        [DllImport(NanomsgDll, EntryPoint = "nng_listener_setopt", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptStringBuilder(uint lid, [MarshalAs(LPStr)] string name
            , [MarshalAs(LPStr)] StringBuilder value, ulong length);

        [DllImport(NanomsgDll, EntryPoint = "nng_listener_setopt_ms", CallingConvention = Cdecl)]
        [return: MarshalAs(I4)]
        private static extern int __SetOptDurationMilliseconds(uint lid, [MarshalAs(LPStr)] string name, int value);

        internal uint _lid;

        protected internal override uint Id => _lid;

        public Listener(Socket s, string addr)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => __Create(ref _lid, s.Id, addr));
            Configure(_lid);
        }

        public Listener()
        {
        }

        internal void OnListened(uint lid)
        {
            Close();
            Configure(_lid = lid);
        }

        private void Configure(uint lid)
        {
            /* Do not be fooled by the names here. They may look the same between Listener and
             * Dialer EndPoints, however, they are anything but. They are proxies for P/Invoke
             * DLL calls! And then specific to Listener and Dialer! */

            // This is pretty amazing!
            SetDelegates(
                flags => __Start(lid, flags)
                , () => __Close(lid)
            );

            var opt = ProtectedOptions;

            // Once again, pretty amazing!
            opt.SetGetters(
                (string name, ref int value) => __GetOptInt32(lid, name, ref value)
                , (string name, ref ulong value) => __GetOptUInt64(lid, name, ref value)
                , (string name, StringBuilder value, ref ulong length) => __GetOptStringBuilder(
                    lid, name, value, ref length)
                , (string name, ref int value) => __GetOptDurationMilliseconds(lid, name, ref value)
            );

            // Ditto amazing!
            opt.SetSetters(
                (name, value) => __SetOptInt32(lid, name, value)
                , (name, sz) => __SetOptUInt64(lid, name, sz)
                , (name, value, length) => __SetOptStringBuilder(lid, name, new StringBuilder(value), length)
                , (name, value) => __SetOptDurationMilliseconds(lid, name, value)
            );
        }

        public override void Close()
        {
            base.Close();
            _lid = 0;
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

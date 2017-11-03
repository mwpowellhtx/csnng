using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using static Marshal;

    public class InProcessAddressFamilyView : PathAddressFamilyView, IInProcessAddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.InProcess;

        internal unsafe InProcessAddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            fixed (byte* p = @base.InProc.Path)
            {
                Path = PtrToStringAnsi((IntPtr) p);
            }
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using static Marshal;

    public class InterProcessAddressFamilyView : PathAddressFamilyView, IInterProcessAddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.InterProcess;

        internal unsafe InterProcessAddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            fixed (byte* p = @base.InProc.Path)
            {
                Path = PtrToStringAnsi((IntPtr) p);
            }
        }
    }
}

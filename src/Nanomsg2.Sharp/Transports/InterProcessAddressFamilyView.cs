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

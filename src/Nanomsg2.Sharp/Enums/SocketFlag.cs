using System;

namespace Nanomsg2.Sharp
{
    [Flags]
    public enum SocketFlag : int
    {
        None = 0x0,
        Alloc = 0x1, // = ::NNG_FLAG_ALLOC,
        NonBlock = 0x2, // = ::NNG_FLAG_NONBLOCK
    };
}

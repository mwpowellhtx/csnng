namespace Nanomsg2.Sharp
{
    // ReSharper disable InconsistentNaming
    public enum SocketAddressFamily : ushort
    {
        Unspecified = 0, // ::NNG_AF_UNSPEC,
        InProcess, // ::NNG_AF_INPROC,
        InterProcess, // ::NNG_AF_IPC,
        IPv4, // ::NNG_AF_INET,
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        IPv6, // ::NNG_AF_INET6,
        ZeroTier, // ::NNG_AF_ZT (ZeroTier)
    };
}

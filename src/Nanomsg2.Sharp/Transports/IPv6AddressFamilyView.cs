namespace Nanomsg2.Sharp
{
    using Collections.Generic;

    public class IPv6AddressFamilyView : AddressFamilyView<SOCKADDR>, IIPv6AddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.IPv6;

        public ushort Port { get; set; }

        public IFixedSizeList<byte> Bytes { get; }

        public IFixedSizeList<ushort> Shorts { get; }

        public IFixedSizeList<uint> Ints { get; }

        public IFixedSizeList<ulong> Longs { get; }

        internal IPv6AddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            Port = @base.IPv6.Port;

            Bytes = new FixedSizeList<byte>(
                @base.IPv6.Addr.Byte0, @base.IPv6.Addr.Byte1
                , @base.IPv6.Addr.Byte2, @base.IPv6.Addr.Byte3
                , @base.IPv6.Addr.Byte4, @base.IPv6.Addr.Byte5
                , @base.IPv6.Addr.Byte6, @base.IPv6.Addr.Byte7
                , @base.IPv6.Addr.Byte8, @base.IPv6.Addr.Byte9
                , @base.IPv6.Addr.ByteA, @base.IPv6.Addr.ByteB
                , @base.IPv6.Addr.ByteC, @base.IPv6.Addr.ByteD
                , @base.IPv6.Addr.ByteE, @base.IPv6.Addr.ByteF
            );

            Shorts = new FixedSizeList<ushort>(
                @base.IPv6.Addr16.UShort0, @base.IPv6.Addr16.UShort1
                , @base.IPv6.Addr16.UShort2, @base.IPv6.Addr16.UShort3
                , @base.IPv6.Addr16.UShort4, @base.IPv6.Addr16.UShort5
                , @base.IPv6.Addr16.UShort6, @base.IPv6.Addr16.UShort7
            );

            Ints = new FixedSizeList<uint>(
                @base.IPv6.Addr32.UInt0, @base.IPv6.Addr32.UInt1
                , @base.IPv6.Addr32.UInt2, @base.IPv6.Addr32.UInt3
            );

            Longs = new FixedSizeList<ulong>(
                @base.IPv6.Addr64.ULong0, @base.IPv6.Addr64.ULong1
            );
        }
    }
}

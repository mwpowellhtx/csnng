namespace Nanomsg2.Sharp
{
    using Collections.Generic;

    public interface IIPv6AddressFamilyView : IAddressFamilyView, IHavePort
    {
        IFixedSizeList<byte> Bytes { get; }

        IFixedSizeList<ushort> Shorts { get; }

        IFixedSizeList<uint> Ints { get; }

        IFixedSizeList<ulong> Longs { get; }
    }
}

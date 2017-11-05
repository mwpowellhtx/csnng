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

using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using static LayoutKind;

    [StructLayout(Explicit)]
    public struct SOCKADDR_Unspecified
    {
        [FieldOffset(0)]
        public ushort Family;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_Path
    {
        [FieldOffset(0)]
        public ushort Family;

        /* We have to go with Byte (1-byte Chars) here, instead of Char, as in the
         * C/C++, which is being interpreted as multibyte (2-byte) wide characters. */
        [FieldOffset(sizeof(ushort))]
        public unsafe fixed byte Path [128];
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv4
    {
        [FieldOffset(0)]
        public ushort Family;

        [FieldOffset(sizeof(ushort))]
        public ushort Port;

        [FieldOffset(sizeof(ushort) * 2)]
        public uint Address;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv6_Addr
    {
        [FieldOffset(0)]
        public unsafe fixed byte Data [16];

        [FieldOffset(0)]
        public byte ByteF;

        [FieldOffset(1)]
        public byte ByteE;

        [FieldOffset(2)]
        public byte ByteD;

        [FieldOffset(3)]
        public byte ByteC;

        [FieldOffset(4)]
        public byte ByteB;

        [FieldOffset(5)]
        public byte ByteA;

        [FieldOffset(6)]
        public byte Byte9;

        [FieldOffset(7)]
        public byte Byte8;

        [FieldOffset(8)]
        public byte Byte7;

        [FieldOffset(9)]
        public byte Byte6;

        [FieldOffset(10)]
        public byte Byte5;

        [FieldOffset(11)]
        public byte Byte4;

        [FieldOffset(12)]
        public byte Byte3;

        [FieldOffset(13)]
        public byte Byte2;

        [FieldOffset(14)]
        public byte Byte1;

        [FieldOffset(15)]
        public byte Byte0;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv6_Addr16
    {
        [FieldOffset(0)]
        public unsafe fixed ushort Data [8];

        [FieldOffset(0)]
        public ushort UShort7;

        [FieldOffset(sizeof(ushort))]
        public ushort UShort6;

        [FieldOffset(sizeof(ushort) * 2)]
        public ushort UShort5;

        [FieldOffset(sizeof(ushort) * 3)]
        public ushort UShort4;

        [FieldOffset(sizeof(ushort) * 4)]
        public ushort UShort3;

        [FieldOffset(sizeof(ushort) * 5)]
        public ushort UShort2;

        [FieldOffset(sizeof(ushort) * 6)]
        public ushort UShort1;

        [FieldOffset(sizeof(ushort) * 7)]
        public ushort UShort0;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv6_Addr32
    {
        [FieldOffset(0)]
        public unsafe fixed uint Data [4];

        [FieldOffset(0)]
        public uint UInt3;

        [FieldOffset(sizeof(uint))]
        public uint UInt2;

        [FieldOffset(sizeof(uint) * 2)]
        public uint UInt1;

        [FieldOffset(sizeof(uint) * 3)]
        public uint UInt0;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv6_Addr64
    {
        [FieldOffset(0)]
        public unsafe fixed ulong Data [2];

        [FieldOffset(0)]
        public ulong ULong1;

        [FieldOffset(sizeof(ulong))]
        public ulong ULong0;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_IPv6
    {
        [FieldOffset(0)]
        public ushort Family;

        [FieldOffset(sizeof(ushort))]
        public ushort Port;

        [FieldOffset(sizeof(ushort) * 2)]
        public SOCKADDR_IPv6_Addr Addr;

        [FieldOffset(sizeof(ushort) * 2)]
        public SOCKADDR_IPv6_Addr16 Addr16;

        [FieldOffset(sizeof(ushort) * 2)]
        public SOCKADDR_IPv6_Addr32 Addr32;

        [FieldOffset(sizeof(ushort) * 2)]
        public SOCKADDR_IPv6_Addr64 Addr64;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR_ZeroTier
    {
        [FieldOffset(0)]
        public ushort Family;

        [FieldOffset(sizeof(ushort))]
        public ulong NetworkId;

        [FieldOffset(sizeof(ushort) + sizeof(ulong))]
        public ulong NodeId;

        [FieldOffset(sizeof(ushort) + sizeof(ulong) * 2)]
        public ushort Port;
    }

    [StructLayout(Explicit, Pack = 1)]
    public struct SOCKADDR
    {
        [FieldOffset(0)]
        public ushort Family;

        [FieldOffset(0)]
        public SOCKADDR_Unspecified Unspec;

        [FieldOffset(0)]
        public SOCKADDR_Path InProc;

        [FieldOffset(0)]
        public SOCKADDR_Path InterProc;

        [FieldOffset(0)]
        public SOCKADDR_IPv4 IPv4;

        [FieldOffset(0)]
        public SOCKADDR_IPv6 IPv6;

        [FieldOffset(0)]
        public SOCKADDR_ZeroTier ZeroTier;
    }
}

using System;
using System.Runtime.InteropServices;

// TODO: TBD: may refactor parts of this into the core API
namespace Nanomsg2.Sharp
{
    using static LayoutKind;

    [StructLayout(Explicit, Pack = 1)]
    internal struct POLLFD
    {
        [FieldOffset(0)]
        public ulong Fd;

        [FieldOffset(sizeof(ulong))]
        public short Events;

        [FieldOffset(sizeof(ulong) + sizeof(short))]
        public short Revents;

        internal POLLFD(int fd, short e, short re)
            : this((ulong) fd, e, re)
        {
        }

        internal POLLFD(ulong fd, short e, short re)
        {
            Fd = fd;
            Events = e;
            Revents = re;
        }
    }

    [Flags]
    internal enum PollEvent : short
    {
        ReaddNormal = 0x0100,
        ReadBand = 0x0200,
        In = ReaddNormal | ReadBand,
        HighPriority = 0x0400,
        WriteNormal = 0x0010,
        Out = WriteNormal,
        WriteBand = 0x0020,
        Error = 0x0001,
        Hangup = 0x0002,
        Invalid = 0x0004,
    }

    internal static class PollExtensionMethods
    {
        public static PollEvent ToPollEvent(this short value) => (PollEvent) value;

        public static short ToShort(this PollEvent value) => (short) value;
    }
}

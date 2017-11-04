using System.Collections.Generic;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using static SocketFlag;

    public interface ISender
    {
        void Send(Message message, SocketFlag flags = None);

        void Send(IEnumerable<byte> bytes, SocketFlag flags = None);

        void Send(IEnumerable<byte> bytes, int count, SocketFlag flags = None);

        void Send(IEnumerable<byte> bytes, long count, SocketFlag flags = None);

        void Send(string s, SocketFlag flags = None);

        void Send(string s, int length, SocketFlag flags = None);
    }
}

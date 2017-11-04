using System.Collections.Generic;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using static SocketFlag;

    public interface IReceiver
    {
        Message ReceiveMessage(SocketFlag flags = None);

        IEnumerable<byte> ReceiveBytes(ref int count, SocketFlag flags = None);

        bool TryReceive(Message m, SocketFlag flags = None);

        bool TryReceive(ICollection<byte> bytes, ref int count, SocketFlag flags = None);
    }
}

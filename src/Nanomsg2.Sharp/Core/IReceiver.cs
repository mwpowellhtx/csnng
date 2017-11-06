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

        // TODO: TBD: for now, using a direct injection of the service;
        // TODO: TBD: think about how that looks for true async functionality.
        void ReceiveAsync(BasicAsyncService svc);
    }
}

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

        // TODO: TBD: for now, using a direct injection of the service;
        // TODO: TBD: think about how that looks for true async functionality.
        void SendAsync(BasicAsyncService svc);
    }
}
